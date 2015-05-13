$(document).ready(function () {
    $('#submit').click(function () {

        var img = $(document.createElement('img'))
            .addClass('img-responsive')
            .css('width', '40px')
            .css('height', '40px')
            .attr('src', '/Content/img/ajax-loader.gif');
        var text = $(document.createElement('h3')).text('Processing request');

        var colImg = $(document.createElement('div'))
            .addClass('col-md-1 col-md-offset-5');
        var colText = $(document.createElement('div'))
            .addClass('col-md-5 col-md-offset-4');

        var row = $(document.createElement('div'))
            .addClass('row');

        var row1 = $(document.createElement('div'))
            .addClass('row');

        colImg.append(img);
        colText.append(text);
        row.append(colImg);
        row1.append(colText);
        $('#main-form').empty();
        $('#main-form').append(row);
        $('#main-form').append(row1);
    });
    $('#file').change(function () {
        if (this.files && this.files[0]) {
            $('#submit').removeAttr('disabled');
            var fr = new FileReader();

            fr.onload = function (e) {
                var image = new Image();
                image.src = e.target.result;
                image.onload = function () {
                    var w = this.width;
                    var h = this.height;
                    $('#width').val(w);
                    $('#height').val(h);
                }
                $('#img').attr('src', e.target.result)
                    .css('visibility', 'visible');
            }

            fr.readAsDataURL(this.files[0]);
        }
    })
});