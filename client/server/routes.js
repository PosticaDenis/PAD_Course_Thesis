var path = require('path');

function Routes (app) {
    'use strict';

    app.get('/', function (req, res) {
        res.sendFile(path.resolve(__dirname + './../public/index.html'));
    });

    app.get('/test', function (req, res) {
        res.send('da');
    })

    var mainPage = require('./app/ctrls/main-page/main-page.js');
    app.get('/basic-info', mainPage.getBasicInfoApi);
    app.post('/basic-info', mainPage.updateBasicInfoApi);

}

module.exports.routes = Routes;
