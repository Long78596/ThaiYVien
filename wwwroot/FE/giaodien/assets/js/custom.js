/* Common function */
var waitForEl = function (selector, callback) {
    if (jQuery(selector).length) {
        callback();
    } else {
        setTimeout(function () {
            waitForEl(selector, callback);
        }, 100);
    }
};

function certfSliderAuto() {
    jQuery('.sec-why-slider .tabbed-content > i.icon-angle-right').trigger('click');
    setTimeout(certfSliderAuto, 6000);
}

function openMegaMenuItem(linkText) {
    if (jQuery(".row-megamenu .accordion-title:contains('" + linkText + "')").length > 0) {
        jQuery(".row-megamenu .accordion-title:contains('" + linkText + "')").trigger('click');
    }
}

function megaMenuAutoPlayVideo(count) {
    if (count <= 0) {
        return;
    }
    if (jQuery(".mfp-container").length == 0) {
        setTimeout(autoplayVideo, 2000, count - 1);
    } else {
        var iframe = document.querySelector(".mfp-container iframe");
        var player = new Vimeo.Player(iframe);

        player.setVolume(0);
        player.play();
    }
}

function initServiceSlider() {
    jQuery('.sec-service .row-slider').lazyFlickity({
        cellAlign: 'left',
        groupCells: 1,
        prevNextButtons: true,
        pageDots: false,
        autoPlay: true,
        lazyLoad: true,
        freeScroll: true,
        wrapAround: true
    });
}

function bookingPhoneInputEvent() {
    jQuery("#input-sdt-dat-lich").change(function (e) {
        var bookPhone = jQuery("#input-sdt-dat-lich").val();
        jQuery("#btn-dat-lich").attr("href", "/dat-lich-truc-tuyen?phone=" + bookPhone);

    });
}

function initSliderAddress() {
    jQuery('.sec-he-thong .address-row').lazyFlickity({
        cellAlign: 'left',
        groupCells: 1,
        prevNextButtons: true,
        pageDots: false,
        autoPlay: 3000,
        lazyLoad: true,
        freeScroll: true,
        wrapAround: true,
        pauseAutoPlayOnHover: false
    });
}

function sliderTrungTamMau_ChangeImg(data, index) {
    var element = data.cells[index].element;
    var imageUrl = element.querySelector('img').src;
    jQuery('.slide-holder img').attr('src', imageUrl);
}

function sliderTrungTamMauEvent() {
    jQuery('.slider-trung-tam > .slider').on('change.flickity', function (event, index) {
        var data = jQuery(this).data('flickity');
        sliderTrungTamMau_ChangeImg(data, index);
    });
}

document.addEventListener("DOMContentLoaded", function () {
    //init slider
    //console.log("init slider");
    //initServiceSlider();

    initSliderAddress();

    //sliderTrungTamMauEvent();

    bookingPhoneInputEvent();

    //mega menu
    // jQuery('#header .header-nav .mega-menu-wrap ul.max-mega-menu > li > a').on('click', function(e){
    //     e.preventDefault();
    //     let linkText = jQuery(this).text();
    //     openMegaMenuItem(linkText);
    // });

    //mega menu media
    // jQuery(".row-megamenu .list-media .box-image").siblings(".box-text").on("click", function(e){
    //     e.preventDefault();
    //     jQuery(this).find(".video-button-wrapper a").trigger("click");
    // });

    // jQuery(".row-megamenu .list-media .box-text-inner .link_play_video").on("click", function(e){
    //     e.preventDefault();
    //     jQuery(this).parent().parent().find(".video-button-wrapper a").trigger("click");
    // });

    //mega menu video
    // jQuery(".row-megamenu .list-video .post-item").each(function(){
    //     if(jQuery(this).find('.video-button-wrapper > a.open-video').length == 0) return;
    //     let url = jQuery(this).find('.video-button-wrapper > a.open-video').first().attr('href').toLowerCase();
    //     const regex = /vimeo\.com\/([0-9]+)/;

    //     var match = regex.exec(url);
    //     if(match != null){
    //         let videoId = match[1];
    //         let videoThumbUrl = 'https://vumbnail.com/' + videoId + '.jpg';
    //         jQuery(this).find('.box-image picture > source').attr('srcset', videoThumbUrl);
    //         jQuery(this).find('.box-image picture > img').attr('src', videoThumbUrl);
    //     }

    //     //click open video btn
    //     jQuery(this).find('.video-button-wrapper > a.open-video').first().on("click", function(){
    //         setTimeout(megaMenuAutoPlayVideo, 3000, 10);
    //     });
    //     jQuery(this).find('button.link_play_video').first().on("click", function(e){
    //         jQuery(this).parent().parent().find('.video-button-wrapper > a.open-video').first().trigger('click');
    //     });
    // });

    //partner-slider
    // jQuery('.row-partner-slider').each(function(){
    //     //console.log("init slider");
    //     jQuery(this).find('style[scope="scope"]').remove();
    //     jQuery(this).flickity({
    //         cellAlign:'left',
    //         contain:true,
    //         groupCells:1,
    //         prevNextButtons:true,
    //         pageDots:false,
    //         wrapAround: true,
    //         autoPlay: true,
    //         autoPlay: 3000,
    //         selectedAttraction: 0.001,
    //         friction: 1,
    //     });
    // });

    //partner-slider in About us page
    // jQuery('.pnl-partner > .panel-inner').each(function(){
    //     jQuery(this).flickity({
    //         cellAlign:'left',
    //         contain:true,
    //         groupCells:1,
    //         prevNextButtons:true,
    //         pageDots:false,
    //         wrapAround: true,
    //         autoPlay: true,
    //         autoPlay: 1500,
    //         selectedAttraction: 0.001,
    //         friction: 1,
    //     });
    // });

    //mega menu - product - vote
    // jQuery('.row-megamenu .list-product .vote-wrapper').each(function(){
    //     let vote = parseInt(jQuery(this).attr("data-vote"));
    //     if(!isNaN(vote) && vote > 0 && vote < 5){
    //         //console.log("Vote: " + vote);
    //         for(let i=vote+1;i<6;i++){
    //             //console.log("i: " + i);
    //             jQuery(this).find("i:nth-child(" + i + ")").css("color", "#6c757d");
    //             //console.log(jQuery(this).find("i:nth-child(" + i + ")"));
    //         }
    //     }
    // });

    // jQuery('.sec-why-slider .tabbed-content ul.nav').before('<i class="icon-angle-left"></i>');
    // jQuery('.sec-why-slider .tabbed-content ul.nav').after('<i class="icon-angle-right"></i>');

    // jQuery('.sec-why-slider .tabbed-content > i.icon-angle-right').click(function() {
    //     var next = jQuery('.sec-why-slider .tabbed-content ul.nav > li.active').first().next('li');
    //     if(next.length){
    //         next.find('a').trigger('click');
    //     }else{
    //         jQuery('.sec-why-slider .tabbed-content ul.nav a:first').trigger('click');
    //     }
    // });

    // jQuery('.sec-why-slider .tabbed-content > i.icon-angle-left').click(function() {
    //     var prev = jQuery('.sec-why-slider .tabbed-content ul.nav > li.active').first().prev('li');
    //     if(prev.length){
    //         prev.find('a').trigger('click');
    //     }else{
    //         jQuery('.sec-why-slider .tabbed-content ul.nav a:last').trigger('click');
    //     }
    // });

    // setTimeout(certfSliderAuto, 6000);

    //Setup our on formSumbit Listener. 
    // jQuery( document ).on( 'nfFormSubmitResponse', function() {
    //     jQuery.magnificPopup.open({
    //         midClick: true,
    //         removalDelay: 300,
    //         items: {
    //             src: '#popup-form-success-msg',
    //             type: 'inline'
    //         }
    //     })
    // });
});