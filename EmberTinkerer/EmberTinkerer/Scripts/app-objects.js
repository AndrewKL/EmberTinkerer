/* Objects
*inject Tinkerer.loginUrl
*/

App.User = Ember.Object.extend({
    loginFailed: false,
    registrationFailed: false,
    login: function () {
        $().post(Tinkerer.loginUrl, this).then(function (response) {
            console.log("login recieved");
            console.log(response);
        });
    }
});

App.Project = Ember.Object.extend({
    find: function (id) {
        $.getJSON(Tinkerer.getURL + "/" + id).then(function (response) {
            //console.log("project w/ id: " + id);
            return App.Project.create(response);
        });
    },
    
    update: function () {
        //console.log("updating");
        //console.log(this);
        var data = {
            Id: 'projects/' + this.id,
            name: this.name,
            description: this.description,
            html: this.html,
            javascript: this.javascript,
        };
        if (this.id == 'new') {
            var project = this;
            console.log("adding new project");
            console.log(this);
            $.post(Tinkerer.addURL, data, function (data) {
                project.id = data.Id.substring(data.Id.indexOf('/') + 1, data.Id.length);
                console.log('new id: ' + project.id);
            });
        } else {
            this.Id = 'project/' + this.id;
            console.log('updating');
            console.log(this);
            $.post(Tinkerer.updateURL, data);
        }
    },

    generateFullHtml: function () {
        var source = $("#full-html-template").html();
        var template = Handlebars.compile(source);
        return template(this);
    }.property("Javascript", "html")
});