/* Objects
*inject Tinkerer.loginUrl
*/

App.User = Ember.Object.extend({
    isLoggedIn: false,
    loginFailed: false,
    loginSucceeded: false,
    registrationFailed: false,
    registrationSucceeded: false,
    test: "asdfasdfasdf",
    login: function () {
        return $.ajax({
            type: "POST",
            url: Tinkerer.loginURL,
            data: {
                Username: this.loginUsername,
                Password: this.loginPassword
            },
        }); 
    },
    logout: function () {
        return $.ajax({
            type: "POST",
            url: Tinkerer.logoutURL,
        });
    },
    register: function () {
        console.log("register user");
        console.log(this);
        return $.ajax({
            type: "POST",
            url: Tinkerer.registerURL,
            data: {
                Username: this.registerUsername,
                Password: this.registerPassword,
                Email: this.registerEmail
            },
        });
    },
});

App.Project = Ember.Object.extend({
    find: function (id) {
        $.getJSON(Tinkerer.getURL + "/" + id).then(function (response) {
            return App.Project.create(response);
        });
    },
    
    update: function () {
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
            return $.post(Tinkerer.addURL, data, function (data) {
                project.id = data.Id.substring(data.Id.indexOf('/') + 1, data.Id.length);
                console.log('new id: ' + project.id);
            });
        } else {
            this.Id = 'projects/' + this.id;
            console.log('updating');
            console.log(this);
            return $.post(Tinkerer.updateURL, data);
        }
    },

    generateFullHtml: function () {
        var source = $("#full-html-template").html();
        var template = Handlebars.compile(source);
        return template(this);
    }.property("Javascript", "html")
});