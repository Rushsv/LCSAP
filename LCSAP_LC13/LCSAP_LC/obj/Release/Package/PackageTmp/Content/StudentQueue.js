
function setTime() {
    ++totalSeconds;
    secondsLabel.innerHTML = pad(totalSeconds % 60);
    minutesLabel.innerHTML = pad(parseInt(totalSeconds / 60));
}

function setHourTime() {
    var seconds = totalSeconds;
    hoursLabel.innerHTML = pad(seconds / 3600);
    seconds = seconds % 3600;
    minutesLabel.innerHTML = pad(parseInt(seconds / 60));
    secondsLabel.innerHTML = pad(seconds % 60);
}

function pad(val) {
    var valString = val + "";
    if (valString.length < 2) {
        return "0" + valString;
    }
    else {
        return valString;
    }
}
