var MovieModule = (function ($, pub) {

    $("#dialog").dialog({
        modal: true,
        autoOpen: false,
        position: {
            my: "center",
            at: "center",
            of: $("#jsgrid")
        }
    });

    //function LoadGrid() {
    var grid = $("#jsGrid").jsGrid({
        height: "auto",
        width: "100%",

        filtering: true,
        //editing: true,
        //sorting: true,

        paging: true,
        pageLoading: true,
        pageButtonCount: 5,
        pagerContainer: "#externalPager",
        pagerFormat: "current page: {pageIndex} &nbsp;&nbsp; {first} {prev} {pages} {next} {last} {pageCount} &nbsp;&nbsp; total items: {itemCount}",
        pagePrevText: "<",
        pageNextText: ">",
        pageFirstText: "<<",
        pageLastText: ">>",
        pageNavigatorNextText: "&#8230;",
        pageNavigatorPrevText: "&#8230;",
        pageSize: 10,
        deleteConfirm: function (item) {
            return "The movie \"" + item.Caption + "\" will be removed. Are you sure?";
        },

        controller: {
            loadData: function (filter) {
                var d = $.Deferred();
                return $.ajax({
                    type: "GET",
                    url: "/MovieLibrary/GetAllActiveMovies",
                    data: filter,
                    dataType: "json"
                }).done(function (response) {
                    console.log(response);
                    d.resolve({ data: JSON.stringify(response.data), itemsCount: response.itemsCount });
                }).fail(function () {
                    console.log("Failed to load movies data!");
                });
            },
        },

        fields: [         
            { name: "Caption", type: "text", width: 130, autosearch: true, align: "left" },
            { name: "ReleaseYear", title: "Release Year", type: "number", width: 60, autosearch: true, align: "center" },
            { name: "SubmittedBy", title: "Submitted By", type: "text", width: 100, autosearch: false, align: "center" },
            { name: "NumberOfAvailableCopies", title: "Number of Available Copies", type: "number", width: 60, autosearch: true, align: "center" },
            //{   name: "Image",
            //    itemTemplate: function (value, item) {
            //        console.log("Image:" + item.Image);
            //        console.log("Val:" + value);
            //        val = "data:image/png;base64," + Convert.ToBase64String(item);
            //        return $("<img>").attr("src", val).css({ height: 50, width: 50 }).on("click", function () {
            //            $("#imagePreview").attr("src", item.Image);
            //            $("#dialog").dialog("open");
            //        });
            //    },
            //    align: "center",
            //    width: 120
            //},
            {
                name: "Director",
                title: "Directors",
                type: "text",
                width: 120,
                autosearch: true,
                align: "center",
                itemTemplate: function (value, item) {
                    var $nestedGridD = $("<div class='myjsgrid'>");
                    $nestedGridD.click(function (e) {
                        e.stopPropagation();
                    });
                    $nestedGridD.jsGrid({
                        //width: 200,
                        //height: "auto",
                        heading: false,
                        selecting: false,
                        noDataContent: "",
                        //inserting: true,
                        //editing: true,
                        data: item.MovieDirectors,
                        fields: [
                            //{ name: "DirectorID", width: 200 },
                            { name: "Name", width: 200 }
                        ],
                        rowClick: function (args) {
                            if (this.editing) {
                                this.editItem($(args.event.target).closest("tr"));
                                args.event.stopPropagation();
                            }
                        }
                    });
                    return $nestedGridD;
                }
            },
            {
                name: "Genre",
                title: "Genres",
                type: "text",
                width: 80,
                autosearch: true,
                align: "center",
                itemTemplate: function (value, item) {
                    var $nestedGridG = $("<div class='myjsgrid'>");
                    $nestedGridG.click(function (e) {
                        e.stopPropagation();
                    });
                    $nestedGridG.jsGrid({
                        //width: 200,
                        //height: "auto",
                        heading: false,
                        selecting: false,
                        noDataContent: "",
                        //inserting: true,
                        //editing: true,
                        data: item.MovieGenres,
                        fields: [
                            //{ name: "GenreID", width: 200 },
                            { name: "Name", width: 200 }
                        ],
                        rowClick: function (args) {
                            if (this.editing) {
                                this.editItem($(args.event.target).closest("tr"));
                                args.event.stopPropagation();
                            }
                        }
                    });
                    return $nestedGridG;
                }
            },
            {
                type: "control",
                modeSwitchButton: false,
                headerTemplate: function () {
                    return $("<button class='btn btn-primary'>").attr("type", "button").text("Add")
                        .on("click", function () {
                            window.location.href = "/MovieLibrary/AddMovie";
                        });
                },
                itemTemplate: function (value, item) {
                    var editDeleteBtn = $('<input class="jsgrid-button jsgrid-edit-button" type="button" title="Edit"><input class="jsgrid-button jsgrid-delete-button" type="button" title="Delete">')
                        .on('click', function (e) {
                            console.clear();
                            if (e.target.title == 'Edit') {
                                e.stopPropagation();
                                window.location.href = "/MovieLibrary/MovieEdit/?id=" + item.MovieID;
                            } else {
                                return $.ajax({
                                    type: "GET",
                                    url: "/MovieLibrary/MovieDeleteGrid",
                                    data: { id: item.MovieID }
                                }).done(function (response) {
                                    console.log(response);
                                    if (response) {
                                        $("#jsGrid").jsGrid("search");
                                        console.log("Movie successfully deleted!");
                                    }
                                    else {
                                        console.log("Backend error - Failed to delete!");
                                    }                                 
                                }).fail(function () {
                                    console.log("Failed to delete!");
                                });
                            }
                        });

                    return editDeleteBtn;
                }
            }
        ]//end of fields
    });

    function GridInit() {
        $("#jsGrid").jsGrid("search");
    }

    //Expose to public
    //pub.LoadGrid = LoadGrid;
    pub.GridInit = GridInit;
    pub.grid = grid;

    return pub;
}(jQuery, MovieModule || {}));

$(document).ready(function () {
    //MovieModule.LoadGrid();
    MovieModule.grid.controller;
    MovieModule.GridInit();
});