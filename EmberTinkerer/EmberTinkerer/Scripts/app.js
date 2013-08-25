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
			return App.Project.create({
				id: 0,
				html: $("#generic-html-body").text(),
				javascript: $("#generic-javascript").text()
			});
		}

		return $.getJSON(Tinkerer.getURL+"/"+params.project_id).then(function (response) {
		    return App.Project.create(response);
		});
	}
});
App.IndexRoute = Ember.Route.extend({
    model: function (params) {
        return $.getJSON(Tinkerer.getAll).then(function (response) {
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
        return this.modelFor ('project');
    }
});

App.ProjectIndexController = Ember.ObjectController.extend({
    editMode: false,
    
    save: function() {
        this.get('model').update();
        this.set("editMode", false);
    },
    editDescription: function(){
        this.set("editMode", true);
    }
});

App.RunRoute = Ember.Route.extend({
    model: function(params) {
        return this.modelFor ('project');
    }
});
App.RunView = Ember.View.extend({
    didInsertElement: function () {//after iframe is inserted
        var model = this.get('controller').get('model');
        Ember.run.next(this, function () {
            //console.log(model);
            var iFrame = document.getElementById('run-frame');
            iFrame.contentWindow.document.write(model.get('generateFullHtml'));
            //console.log(model.get('generateFullHtml'));
			// more code
		});
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

App.Project = Ember.Object.extend({
	find: function(id){
	    $.getJSON(Tinkerer.getURL + "/" + id).then(function (response) {
	        //console.log("project w/ id: " + id);
	        return App.Project.create(response);
	    });
	},
	update: function() {
	    var data = {
	        id: this.id,
	        name: this.name,
	        description: this.description,
	        html: this.html,
	        javascript: this.javascript,
	    };
	    if (this.id == 0) {
	        var project = this;
	        $.post(Tinkerer.addURL, data, function (data) {
	            project.id = data.id;
	        });
	    } else {
	        $.post(Tinkerer.updateURL, data);
	    }
	    
	},
	
	generateFullHtml: function () {
	    //console.log("generating html");
	    var source = $("#full-html-template").html();
	    //console.log("source: " + source);
        var template = Handlebars.compile(source);
        
        return template( this );
    }.property("javascript","html")
});

Handlebars.registerHelper('scriptBlock', function (script) {
    return '<script>' + script + '</script>';
});
Handlebars.registerHelper('scriptFromSource', function (source) {
    return '<script src="'+source+'"></script>';
});
