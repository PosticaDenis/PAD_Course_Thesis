'use strict';

var mysql = require('mysql');

var connectionParam = require('./../../modules/config.js').DBconf;


function authorization (param, callback) {
    var connection = mysql.createConnection(connectionParam);

    connection.query('SELECT .....', function (err, data) {
        if (err) {
            return callback(err);
        }

        return callback(null);
    });

    connection.end();
}

module.exports.authorizationApi = function authorizationApi (req, res) {
    authorization(req.body, function authHandler(err) {
        if (err) {
            return res.send('oshibka')
            // obrabotat' oshibku
            // mojet bit' chto ugodno:
            // naprimer, oshibka poka v bazu lezli
            // ili ne pravil'nie login/pass
            // kak obrabotat':
            // posilaete soobshenie ob oshibke i status code
            // status codes - propisani v trs
        } else {
            // generiruem token, otpravlyaem na client -> client mutit redirect na main page
        }
    });
};
