App = Ember.Application.create({
	LOG_TRANSITIONS: true,
});

App.Router.map(function() {
	this.resource('index', { path: '/' });
	this.resource('project', { path: '/project/:project_id' }, function() {
		this.resource('run', { path: "/run" });
	});
	this.route('new');
    this.resource('about');
});

App.ProjectRoute = Ember.Route.extend({
	model: function(params) {
		if(params.project_id == undefined){
			console.log("creating new project");
			return App.Project.create({
				id: 7,
				html: $("#generic-html-body").text(),
				javascript: $("#generic-javascript").text()
			});
		}
		console.log("loading existing project");
		return App.Project.create({
			id: 7,
			html: "<html></html>",
			javascript:"alert('this is working');"
		});
	}
});

App.ProjectIndexRoute = Ember.Route.extend({
    model: function(params) {
        console.log("project/index");
        return this.modelFor ('project');
    }
});
App.RunRoute = Ember.Route.extend({
    model: function(params) {
		console.log("project/run");
        return this.modelFor ('project');
    }
});

App.NewRoute = Ember.Route.extend({
  redirect: function() {
    this.transitionTo('project');
  }
});


App.ApplicationView = Ember.View.extend({
  classNames: ['full']
});


App.IndexRoute = Ember.Route.extend({
  model: function() {
    return ['red', 'yellow', 'blue'];
  }
});

App.Project = Ember.Object.extend({
	find: function(id){
		return App.Project.create({
			id: id,
			html: "<html></html>",
			javascript:"alert('this is working');"
		});
	},
	
});
