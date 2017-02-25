$(function() {
    setInterval(function(){
        var _self   = $(".ripple");
        var x       = $(window).width()/2;
        var y       = $(window).height()/2;

        var $effect = $(_self).find('.ripple__effect');
        var w       = $effect.width();
        var h       = $effect.height();

        $effect.css({
            left: x - w / 2,
            top: y - h / 2
        });

        if (!$effect.hasClass('is-show')) {
            $effect.addClass('is-show');
            setTimeout(function() {
                $effect.removeClass('is-show');
            }, 750);
        }
    },1000);
});