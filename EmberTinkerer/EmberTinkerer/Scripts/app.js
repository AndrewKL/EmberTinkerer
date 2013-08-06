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
		console.log(params);
		return $.getJSON(Tinkerer.getURL+"/"+params.project_id).then(function (response) {
		    console.log("getting project w/ id: " + params.project_id);
		    return App.Project.create(response);
		});
	}
});
App.IndexRoute = Ember.Route.extend({
    model: function (params) {
        console.log("index route");
        return $.getJSON(Tinkerer.getAll).then(function (response) {
            console.log("getting all projects");
            var projects = [];

            response.forEach(function (project) {
                projects.push(App.Project.create(project));
            });

            return projects;
        });
    }
});

App.ProjectIndexRoute = Ember.Route.extend({
    model: function(params) {
        console.log("project/index route");
        return this.modelFor ('project');
    }
});
App.RunRoute = Ember.Route.extend({
    model: function(params) {
		console.log("project/run");
        return this.modelFor ('project');
    }
});
/*App.RunView = Ember.View.extend({
		Ember.run.next(this, function(){
			alert("post run");//this.$().isotope({}) // or watever code u want to write
		});
	}
});*///causes rendering to fuck up upon running

App.NewRoute = Ember.Route.extend({
	redirect: function() {
		this.transitionTo('project');
	}
});

App.ApplicationView = Ember.View.extend({
	classNames: ['full']
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