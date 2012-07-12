
if (typeof (DeviceInfo) != "object") {
    DeviceInfo = {}
}
Container = {
    commands: []
};
Container.exec = function () {
    Container.commands.push(arguments);
    var args = Container.commands.shift();
    var uri = [];
    var dict = null;
    for (var i = 1; i < args.length; i++) {
        var arg = args[i];
        if (arg == undefined || arg == null) {
            arg = ""
        }
        if (typeof (arg) == "object") {
            dict = arg
        } else {
            uri.push(encodeURIComponent(arg))
        }
    }
    var actionUri = args[0].replace(".", "/");
    var url = "hybrid://" + actionUri + "/" + uri.join("/");
    if (dict != null) {
        url += "?" + encodeURIComponent(JSON.stringify(dict))
    }

    // pass it on to Notify, notice the difference between this and the Android and iOS platforms
    // on them we can just set the document location and catch the url in the redirect
    // document.location = url;
    window.external.Notify(url);
};

function Accelerometer() {
    Accelerometer.prototype.onAccelerometerSuccess = function (x, y, z) {
    };
    Accelerometer.prototype.onAccelerometerFail = function (message) {
    };
    Accelerometer.prototype.watchAccelerometer = function (onSuccess, onFail) {
        this.onAccelerometerSuccess = onSuccess;
        this.onAccelerometerFail = onFail;
    	Container.exec('accelerometer.start');
    }
    Accelerometer.prototype.cancelAccelerometer = function () {
    	Container.exec('accelerometer.cancel');
    }
}
var accelerometer = new Accelerometer();

function Compass() {
    Compass.prototype.onCompassSuccess = function (heading) {
    };
    Compass.prototype.onCompassFail = function (message) {
    };
    Compass.prototype.watchCompass = function (onSuccess, onFail) {
        this.onCompassSuccess = onSuccess;
        this.onCompassFail = onFail;
    	Container.exec('compass.start');
    }
    Compass.prototype.cancelCompass = function () {
    	Container.exec('compass.cancel');
    }
}
var compass = new Compass();

/*
 * This class provides access to the device media, interfaces to both sound and video
 * @constructor
 */
function MediaPlayer() {
    this.play = function (src) {
        if (src != null) {
            Container.exec('notify.play', src);
        } else {
            Container.exec('notify.play');
        }
    }
    this.pause = function () {
        Container.exec('media.pause');
    }
    this.stop = function () {
        Container.exec('media.stop');
    }
}
var mediaPlayer = new MediaPlayer();

/*
 * This class contains information about any Media errors.
 * @constructor
 */
function MediaError() {
    this.code = null,
    this.message = "";
}

MediaError.MEDIA_ERR_ABORTED = 1;
MediaError.MEDIA_ERR_NETWORK = 2;
MediaError.MEDIA_ERR_DECODE = 3;
MediaError.MEDIA_ERR_NONE_SUPPORTED = 4;

/*
 * This class provides access to notifications on the device.
 */
function Notification() {
    /**
    * Causes the device to vibrate.
    * @param {Integer} mills The number of milliseconds to vibrate for.
    */
    this.vibrate = function (mills) {
        Container.exec('notify.vibrate', mills);
        //document.location = "hybrid://vibrate/1234";
    }
    /*
     * Causes the device to play a sound
     * @param {Integer} count The number of beeps.
     * @param {Integer} volume The volume of the beep.
     */
    this.playSound = function () {
        Container.exec('notify.playSound');
    }
}
var notify = new Notification();
