'use strict';

var mysql = require('mysql');

var connectionParam = require('./../../modules/config.js').DBconf;


function getBasicInfo (param, callback) {
    var connection = mysql.createConnection(connectionParam);

    connection.query('SELECT .....', function (err, data) {
        if (err) {
            return callback(err);
        }

        return callback(null, JSON.stringify(data));
    });

    connection.end();
}

module.exports.getBasicInfoApi = function getBasicInfoApi (req, res) {
    authorization(req.body, function getBasicInfo (err, data) {
        if (err) {
            return res.send('oshibka')
        } else {
            res.header('Content-Type', 'application/json');
            return res.send(data)
        }
    });
};

function updateBasicInfo () {
    // ...
}

module.exports.updateBasicInfoApi = function updateBasicInfoApi () {
    updateBasicInfo();
};
