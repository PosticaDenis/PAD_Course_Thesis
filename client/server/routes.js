var path = require('path');

function Routes (app) {
    'use strict';

    app.get('/', function (req, res) {
        res.sendFile(path.resolve(__dirname + './../public/index.html'));
    });

}

module.exports.routes = Routes;
