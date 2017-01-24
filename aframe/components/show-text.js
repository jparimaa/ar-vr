AFRAME.registerComponent('show-text', {
    schema: {
        target: {type: 'selector'},
        dur: {type: 'number', default: 200} // Add fade out time?
    },

    init: function () {
        var data = this.data;
        var el = this.el;

        el.addEventListener("mouseenter", function () {
            data.target.setAttribute("visible", true);
        });

        el.addEventListener("mouseleave", function () {
            data.target.setAttribute("visible", false);
        });
    },
});
