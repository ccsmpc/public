var chat = $.connection.meetingHub;
var agenda;
var sections = {};
var agendaItems = {};
var agendaItem = {};
var workItem = {};
var workItemAttachments = {};
var agendaItemAttachments = {};
var userName;
chat.client.showModal = function () {
    $('#modalMotion').modal('show');
};
chat.client.hideModal = function () {
    $('#modalMotion').modal('hide');
};
chat.client.clearStage = function (voteStage, vote) {
    switch (voteStage) {
        case "3":
            $('#secondaryAmendmentText').empty();
            $('#modalTitle').append("The secondary amendment " + vote + ". <br />");
            break;
        case "2":
            $('#primaryAmendmentText').empty();
            $('#modalTitle').append("The primary amendment " + vote + ".<br />");
            break;
        case "1":
            $('#modalTitle').append("The motion has " + vote + ".");
            break;
        default:
            break;
    }
};
chat.client.clearModal = function () {
    $('#modalTitle').empty();
    $('#motionText').empty();
    $('#primaryAmendmentText').empty();
    $('#secondaryAmendmentText').empty();
    $('#modalFooter').empty();
};
chat.client.clearFooter = function () {
    $('#modalFooter').empty();
};
chat.client.showMotion = function (motionHeader, motionText) {
    $('#modalTitle').empty().append(motionHeader);
    $('#motionText').empty().append(motionText);
};
chat.client.enableVote = function (currentVoteItemId, phase) {
    $('#modalFooter').empty().append('<button class="btn btn-success" type="button" id="voteYes">Vote YES</button> <button class="btn btn-warning" type=button id="voteNo">Vote NO</button>');
    voteItemId = currentVoteItemId;
    votePhase = phase;
};
chat.client.showCurrentMotion = function (currentMotion) {
    $('#modalTitle').empty().append('<h2>The motion before the board is:</h2>');
    $('#modalBody').empty().append(currentMotion);
    $('#modalFooter').empty();
    $('#modalMotion').modal('show');
};
chat.client.hideCurrentMotion = function () {
    $('#modalMotion').modal('hide');
};
chat.client.showCurrentVoting = function (currentVoteItemId) {
    $('#modalTitle').empty().append('<h2>The board is voting on the following:</h2>');
    voteItemId = currentVoteItemId;
};
chat.client.enableVoteButtons = function () {
    $('#modalFooter').empty().append("<button type='button' class='btn btn-success btn-lg' id='voteYes'>YES</button><button type='button' class='btn btn-danger btn-lg' id='voteNo'>NO</button>");
};
chat.client.showVoteResult = function (result, tally) {
    $('#modalTitle').empty().append(result);
    $('#modalFooter').empty().append(tally);
    $('#modalMotion').modal('show');
};
$('.dropdown-toggle').dropdown();
$('#Name').on("change", function () {
    $('#changeName').text($('#Name option:selected').text());
    $('#memberId').val($('#Name option:selected').val());
    $('#nameDropdown').hide();
    $('#memberId').show();
    userName = $('#Name option:selected').text();
    userId = $('#memberId').val();
    chat.server.joinRoom($('#changeName').text(), "BoardMembers");
});
$('#changeName').on("click", function () {
    chat.server.leaveRoom($('#changeName').text(), "BoardMembers");
    userName = "";
    userId = "";
    $('#memberId').hide();
    $('#nameDropdown').show();
});
chat.client.updatePosition = function (currentSection, currentAgendaItem) {
    agendaItems = sections[currentSection].AgendaItems;
    agendaItem = agendaItems[currentAgendaItem];
    $('#sectionTitle').empty().append(sections[currentSection].Description);
    if (agendaItem) {
        $('#agendaItemTitle').empty().append(agendaItem.Description);
        if (agendaItem.WorkItems.length > 0)
            workItem = agendaItem.WorkItems;
        $('#tabs').empty().append('<ul class="tab-links"></ul><div class="tab-content"></div>');
        var tabCounter = 0;
        if (workItem.length > 0) {
            workItemAttachments = workItem[0].Attachments;
            for (i = 0; i < workItemAttachments.length; i++) {
                addTab(tabCounter, workItemAttachments[i].Description, workItemAttachments[i].FilePath);
                tabCounter++;
            }
            ;
        }
        if (agendaItem.Attachments.length > 0) {
            agendaItemAttachments = agendaItem.Attachments;
            for (i = 0; i < agendaItemAttachments.length; i++) {
                addTab(tabCounter, agendaItemAttachments[i].Description, agendaItemAttachments[i].FilePath);
                tabCounter++;
            }
            ;
        }
        $('#tabs').show();
    }
    else {
        $('#agendaItemTitle').empty();
        $('#tabs').hide();
    }
};
chat.client.initializeAgenda = function (agendaId) {
    var currentSection = 0;
    var currentAgendaItem = 0;
    $.ajax({
        type: "GET",
        dataType: 'json',
        contentType: 'application/json',
        url: '/MeetingManager/Meeting/GetAgenda',
        data: { 'agendaId': agendaId },
        cache: false,
        success: function (data) {
            var i;
            agenda = data;
            for (i = 0; i < agenda.SectionHeadings.length; i++) {
                sections[i] = agenda.SectionHeadings[i];
            }
            agendaItems = sections[currentSection].AgendaItems;
            agendaItem = agendaItems[currentAgendaItem];
            $('#meetingTitle').empty().append('<b>' + agenda.Title + '</b>');
            $('#sectionTitle').empty().append(sections[currentSection].Description);
            $('#agendaItemTitle').empty();
            if (agendaItem) {
                $('#agendaItemTitle').empty().append(agendaItem.Description);
                $('#tabs').empty().append('<ul class="tab-links"></ul><div class="tab-content"></div>');
                var tabCounter = 0;
                // add agendaItemAttachemnts to tabs
                if (agendaItem.Attachments.length > 0) {
                    agendaItemAttachments = agendaItem.Attachments;
                    for (i = 0; i < agendaItemAttachments.length; i++) {
                        addTab(tabCounter, agendaItemAttachments[i].Description, agendaItemAttachments[i].FilePath);
                        tabCounter++;
                    }
                    ;
                    // add workItemAttachments to tabs
                    if (agendaItem.WorkItems.length > 0) {
                        workItem = agendaItem.WorkItems;
                        if (workItem.length > 0) {
                            workItemAttachments = workItem[0].Attachments;
                            for (i = 0; i < workItemAttachments.length; i++) {
                                addTab(tabCounter, workItemAttachments[i].Description, workItemAttachments[i].FilePath);
                                tabCounter++;
                            }
                            ;
                        }
                    }
                    $('#tabs').show();
                }
                else {
                    $('#agendaItemTitle').empty();
                    $('#tabs').hide();
                }
            }
            ;
        },
        error: function () { }
    });
};
$.connection.hub.start().done();
;
