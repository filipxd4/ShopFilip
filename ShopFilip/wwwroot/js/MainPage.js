$(document).ready(function () {
    eventThrottler();
});

function lazyLoad() {
    $('.lazy-img').each(function () {
        $(this).attr('src', $(this).data('src'));
        $(this).removeClass('lazy-img');
    })
};

var eventTimeout;
var eventThrottler = function () {
    if (!eventTimeout) {
        eventTimeout = setTimeout(function () {
            eventTimeout = null;
            lazyLoad();
        }, 100);
    }
};