'use strict';

var jwt = require('jsonwebtoken');


var SECRET_KEY = 'secret key'; // should be var SECRET_KEY = fs.readFileSync('private.key');

var allowUrl = [
    '/',
    '/login'
];

function checkAuth (req, res, next) {
    // see what indexOf returns for array
    // ~ make: -(a + 1)
    // example ~5 = -(5 + 1) = -6
    if ( ~allowUrl.indexOf(req.originalUrl) ) {

        console.log('ne proveryaem');
        return next();
    }

    if (req.headers['token']) {

        try {

            var decoded = jwt.verify(req.headers['token'], SECRET_KEY);

        } catch (e) {
            // ne zabud'te sdelat' tut vibros gnevnogo soobsheniya
            console.log('ne nash token - nahui na ulicu etogo script kiddy');
        }

        // v tokene hranit' user id i vse zaprosi v bazu delat' tol'ko po etomu id

        var param = {
            userId: 'AGA',
            role: 'bomj'
        };

        res.append('token', jwt.sign(param, SECRET_KEY));

    } else {
        // tut toje nichego ne delaem krome vibrosa soobsheniya
        console.log('tokena net - redirect k huyam sobachim na glavnuyu');
    }

    next();
}

module.exports.checkAuth = checkAuth;
