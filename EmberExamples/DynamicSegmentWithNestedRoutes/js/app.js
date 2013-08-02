App = Ember.Application.create({
  LOG_TRANSITIONS: true
});

App.Router.map(function() {
  this.resource("posts", { path: "/" }, function() {
    this.resource("post", { path: "/:post_id" }, function() {
      this.route("edit", { path: "/edit" });
    });
    this.route("new", { path: "/new" });
  });
});

App.Store = DS.Store.extend({
  revision: 11,
  adapter: "DS.FixtureAdapter"
});

App.Post = DS.Model.extend({
  title: DS.attr("string"),
  
  isTitleValid: function() {
    return this.get("title.length") > 0;
  }.property("title"),
  
  isTitleInvalid: Ember.computed.not("isTitleValid")
});


App.Post.FIXTURES = [
  { id: 1, title: "hamburger" }  
];

App.PostsIndexController = Ember.ArrayController.extend();
App.PostsIndexRoute = Ember.Route.extend({
  model: function() {
    return App.Post.find();
  }
});
