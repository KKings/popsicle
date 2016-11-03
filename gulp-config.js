module.exports = function () {
    var root = "E:\\Projects";
    var sandbox = root + "\\Instances\\popsicle.local";

    var config = {
        solutionPath: root + "\\popsicle",
        website: "http://popsicle.local",
        websiteRoot: sandbox + "\\Website",
        sitecoreLibraries: sandbox + "\\Website\\bin",
        licensePath: sandbox + "\\Data\\license.xml",
        solutionName: "Popsicle",
        buildConfiguration: "Debug",
        runCleanBuilds: true
    }

    return config;
}