var gulp = require("gulp");
var msbuild = require("gulp-msbuild");
var debug = require("gulp-debug");
var foreach = require("gulp-foreach");
var rename = require("gulp-rename");
var watch = require("gulp-watch");
var merge = require("merge-stream");
var newer = require("gulp-newer");
var util = require("gulp-util");
var runSequence = require("run-sequence");
var path = require("path");
var config = require("./gulp-config.js")();
var nugetRestore = require('gulp-nuget-restore');
var fs = require('fs');

module.exports.config = config;

gulp.task("00-Install", function (callback) {
    config.runCleanBuilds = true;
    return runSequence(
      "01-Nuget-Restore",
      "02-Publish-TDS-Projects",
      "03-Publish-All-Projects",
      callback);
});


gulp.task("01-Nuget-Restore", function (callback) {
    var solution = "./" + config.solutionName + ".sln";
    return gulp
        .src(solution)
        .pipe(nugetRestore());
});


gulp.task("02-Publish-TDS-Projects",
    function () {
        /**
         * Task default configuration
         * @type {Object}
         */
        var buildConfig = {
            config: 'Debug',
            src: ['./Foundation/**/*.scproj', './Feature/**/*.scproj', './Project/**/*.scproj'],
            options: {
                targets: ['Build'],
                configuration: 'Debug',
                logCommand: false,
                verbosity: 'minimal',
                maxcpucount: 0,
                stdout: true,
                stderr: true,
                errorOnFail: true,
                toolsVersion: 14.0,
                properties: {
                    SitecoreDeployFolder: config.websiteRoot,
                    SitecoreWebUrl: config.website,
                    IsDesktopBuild: false,
                    GeneratePackage: false
                }
            },
            deps: []
        };

        return publishTDSProjects(buildConfig);
    });

gulp.task("03-Publish-All-Projects", function (callback) {
    return runSequence(
      "Build-Solution",
      "Publish-Foundation-Projects",
      "Publish-Feature-Projects",
      "Publish-Project-Projects", callback);
});


/*gulp.task("Copy-Local-Assemblies", function () {
    console.log("Copying site assemblies to all local projects");
    var files = config.sitecoreLibraries + "/*#1#*";

    var root = ".";
    var projects = root + "/*#1#code/bin";
    return gulp.src(projects, { base: root })
      .pipe(foreach(function (stream, file) {
          console.log("copying to " + file.path);
          gulp.src(files)
            .pipe(gulp.dest(file.path));
          return stream;
      }));
});*/

var publishProjects = function (location, dest) {
    dest = dest || config.websiteRoot;
    var targets = ["Build"];

    console.log("publish to " + dest + " folder");
    return gulp.src([location + "/**/code/*.csproj"])
      .pipe(foreach(function (stream, file) {
          return stream
            .pipe(debug({ title: "Building project:" }))
            .pipe(msbuild({
                targets: targets,
                configuration: config.buildConfiguration,
                logCommand: false,
                verbosity: "minimal",
                stdout: true,
                errorOnFail: true,
                maxcpucount: 0,
                toolsVersion: 14.0,
                properties: {
                    DeployOnBuild: "true",
                    DeployDefaultTarget: "WebPublish",
                    WebPublishMethod: "FileSystem",
                    DeleteExistingFiles: "false",
                    publishUrl: dest,
                    _FindDependencies: "false"
                }
            }));
      }));
};

var publishTDSProjects = function(config) {
    var options = config.options;
    const src = config.src;

    options.configuration = options.configuration;

    return gulp.src(src)
        .pipe(debug({ title: "Publishing  " }))
        .pipe(msbuild(options));
}


gulp.task("Build-Solution", function () {
    var targets = ["Build"];
    if (config.runCleanBuilds) {
        targets = ["Clean", "Build"];
    }

    return gulp
        .src(["./**/code/*.csproj", "./**/tests/**/*.csproj"])
        .pipe(debug({ title: "Building  " }))
        .pipe(msbuild({
            targets: targets,
            configuration: config.buildConfiguration,
            logCommand: true,
            verbosity: "minimal",
            stdout: true,
            errorOnFail: true,
            maxcpucount: 0,
            toolsVersion: 14.0,
            properties: {
                InstallSitecoreConnector: 'false',
                SitecoreAccessGuid: '06525624-a214-425d-8138-d4556b7cb267',
                DeployOnBuild: "false",
                IsDesktopBuild: "false",
                GeneratePackage: "false"
            }
        }));
});

gulp.task("Publish-Foundation-Projects", function () {
    return publishProjects("./src/Foundation");
});

gulp.task("Publish-Feature-Projects", function () {
    return publishProjects("./src/Feature");
});

gulp.task("Publish-Project-Projects", function () {
    return publishProjects("./src/Project");
});

gulp.task("Publish-Assemblies", function () {
    var root = ".";
    var binFiles = root + "/**/code/**/bin/GE.{Feature,Foundation,Project,Lifesciences}.*.{dll,pdb}";
    var destination = config.websiteRoot + "/bin/";
    return gulp.src(binFiles, { base: root })
      .pipe(rename({ dirname: "" }))
      .pipe(newer(destination))
      .pipe(debug({ title: "Copying " }))
      .pipe(gulp.dest(destination));
});

gulp.task("Publish-All-Views", function() {
    var root = ".";
    var roots = [root + "/**/Views", root + "/**/Areas/**/Views", "!" + root + "/**/obj/**/Views"];
    var files = "/**/*.cshtml";
    var destination = config.websiteRoot + "\\Views";
    return gulp.src(roots, { base: root })
        .pipe(
            foreach(function(stream, file) {
                console.log("Publishing from " + file.path);

                const tempDestination = file.path.indexOf("Areas") >= 0
                    ? config.websiteRoot + "\\" + file.path.substring(file.path.indexOf("Areas"), file.path.length)
                    : destination;

                gulp.src(file.path + files, { base: file.path })
                    .pipe(newer(tempDestination))
                    .pipe(debug({ title: "Copying " }))
                    .pipe(gulp.dest(tempDestination));
                return stream;
            })
        );
});

gulp.task("Publish-All-Configs", function () {
    var root = ".";
    var roots = [root + "/Feature/**/App_Config", root + "/Foundation/**/App_Config", root + "/Project/**/App_Config", "!" + root + "/**/obj/**/App_Config"];
    var files = "/**/*.config";
    var destination = config.websiteRoot + "\\App_Config";
    return gulp.src(roots, { base: root })
        .pipe(foreach(function (stream, file) {
            console.log("Publishing from " + file.path);

            gulp.src(file.path + files, { base: file.path })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));

            return stream;
      })
    );
});

gulp.task("Auto-Publish-Views", function () {
    var root = ".";
    var roots = [root + "/**/Views", root + "/**/Areas/**/Views", "!" + root + "/**/obj/**/Views"];
    var files = "/**/*.cshtml";
    var destination = config.websiteRoot + "\\Views";
    gulp.src(roots, { base: root })
        .pipe(foreach(function (stream, rootFolder) {
            gulp.watch(rootFolder.path + files, function (event) {
                if (event.type === "changed") {
                    console.log("File changed " + event.path);
                  
                    const tempDestination = event.path.indexOf("Areas") >= 0
                        ? config.websiteRoot + "\\" + event.path.substring(event.path.indexOf("Areas"), event.path.lastIndexOf('\\'))
                        : destination;

                    gulp.src(event.path, { base: rootFolder.path })
                        .pipe(newer(tempDestination))
                        .pipe(debug({ title: "Copying " }))
                        .pipe(gulp.dest(tempDestination));
                }
          });

          return stream;
      })
    );
});

gulp.task("Auto-Publish-Assemblies",
    function() {
        var root = ".";
        var roots = [root + "/**/code/**/bin"];
        var files = "/**/GE.{Feature,Foundation,Project,Lifesciences}.*.{dll,pdb}";;
        var destination = config.websiteRoot + "/bin/";
        gulp.src(roots, { base: root })
            .pipe(
                foreach(function(stream, rootFolder) {
                    gulp.watch(rootFolder.path + files,
                        function(event) {
                            if (event.type === "changed") {
                                console.log("DLL Changed " + event.path);
                                gulp.src(event.path, { base: rootFolder.path })
                                    .pipe(gulp.dest(destination));
                            }
                            console.log("published " + event.path);
                        });
                    return stream;
                })
            );
    });