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
			//console.log("creating new project");
			return App.Project.create({
				id: 0,
				html: $("#generic-html-body").text(),
				javascript: $("#generic-javascript").text()
			});
		}
		//console.log("loading existing project");
		//console.log(params);
		return $.getJSON(Tinkerer.getURL+"/"+params.project_id).then(function (response) {
		    //console.log("getting project w/ id: " + params.project_id);
		    return App.Project.create(response);
		});
	}
});
App.IndexRoute = Ember.Route.extend({
    model: function (params) {
        //console.log("index route");
        return $.getJSON(Tinkerer.getAll).then(function (response) {
            //console.log("getting all projects");
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
        //console.log("project/index route");
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
        //console.log(this.modelFor('project'));
        return this.modelFor ('project');
    }
});
App.RunView = Ember.View.extend({
    didInsertElement: function () {
        //console.log(this);
        //console.log(this.get('controller'));
        //console.log(this.get('controller').get('model'));
        var model = this.get('controller').get('model');
        Ember.run.next(this, function () {
            
			alert("post run");// or watever code u want to write
		});
    }
});//causes rendering to fuck up upon running

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
