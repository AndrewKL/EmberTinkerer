App = Ember.Application.create({
	LOG_TRANSITIONS: true,
});

var console = window.console || {
    log: function() {
    }
};

App.Router.map(function() {
	this.resource('index', { path: '/' });
	this.resource('project', { path: '/project/:project_id' }, function() {
		this.resource('run', { path: "/run" });
	});
	this.route('new');
	this.resource('about');
	this.resource('loginregister');
});

App.ApplicationRoute = Ember.Route.extend({
    renderTemplate: function () {
        this.render();
        this.render("user", {
            outlet: "user",
            into: "application"
        });
    },
});
App.UserRoute = Ember.Route.extend({
    model: function(params) {
        return App.User.create();
    }
});

App.IndexRoute = Ember.Route.extend({
    model: function (params) {
        return $.getJSON(Tinkerer.getAll).then(function (response) {
            var projects = [];

            response.forEach(function (project) {
                project.id = project.Id.substring(project.Id.indexOf('/') + 1, project.Id.length);
                projects.push(App.Project.create(project));
            });

            console.log(projects);
            return projects;
        });
    }
});

App.NewRoute = Ember.Route.extend({
    redirect: function () {
        this.transitionTo('project');
    }
});

App.ProjectRoute = Ember.Route.extend({
	model: function(params) {
	    if (params.project_id == undefined || params.project_id=='new') {
			return App.Project.create({
			    id: 'new',
			    name: 'new project',
			    description: 'Add a description.',
				html: $("#generic-html-body").text(),
				javascript: $("#generic-javascript").text()
			});
		}

		return $.getJSON(Tinkerer.getURL+"/"+params.project_id).then(function (response) {
		    console.log(response);
		    return App.Project.create(response);
		});
	}
});

App.ProjectIndexRoute = Ember.Route.extend({
    model: function (params) {
        return this.modelFor('project');
    }
});

App.RunRoute = Ember.Route.extend({
    model: function(params) {
        return this.modelFor('project');
    }
});

App.IndexController = Ember.ObjectController.extend({
    searchText: "",
    
    searchForProjects: function () {
        var controller = this;
        _.debounce(function () {
            $.getJSON(Tinkerer.searchURL, { text: controller.get('searchText') }).then(function (response) {
                var projects = [];
                response.forEach(function(project) {
                    project.id = project.Id.substring(project.Id.indexOf('/') + 1, project.Id.length);
                    projects.push(App.Project.create(project));
                });
                controller.set('model', projects);
            });
        },400)();//runs the function return by debounce
    }.observes("searchText"),
});

App.ProjectIndexController = Ember.ObjectController.extend({
    editMode: false,
    
    save: function () {
        var controller = this;
        var project = this.get('model');
        var oldId = project.id;
        project.update().then(function () {
            if (oldId === 'new') {
                controller.transitionToRoute('project');
            }else {
                controller.set("editMode", false);
            }
        });
    },
    editDescription: function(){
        this.set("editMode", true);
    }
});

App.UserController = Ember.ObjectController.extend({
    setupController: function (controller, song) {
        console.log("settingup usercontroller");
        controller.set('model', App.User.create());
    }
});

App.RunView = Ember.View.extend({
    didInsertElement: function () {//after iframe is inserted
        var model = this.get('controller').get('model');
        Ember.run.next(this, function () {
            var iFrame = document.getElementById('run-frame');
            iFrame.contentWindow.document.write(model.get('generateFullHtml'));
		});
    }
});

App.ApplicationView = Ember.View.extend({
    classNames: ['full']
});

Handlebars.registerHelper('scriptBlock', function (script) {
    return '<script>' + script + '</script>';
});
Handlebars.registerHelper('scriptFromSource', function (source) {
    return '<script src="'+source+'"></script>';
});
