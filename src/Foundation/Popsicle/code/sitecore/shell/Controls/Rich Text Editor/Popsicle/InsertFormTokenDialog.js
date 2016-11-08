define(["sitecore"], function (Sitecore) {
  var view = Sitecore.Definitions.App.extend({
      initialized: function (){
          console.log('welcome to the big leagues');
      }
  });

  return view;
})