using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using AgendaProject.Models;

namespace AgendaProject
{
    public class MeetingHub : Hub
    {
        Dictionary<string,string> userDictionary = new Dictionary<string, string>();

        public void ShowModal()
        {
            Clients.All.showModal();
        }

        public void HideModal()
        {
            Clients.All.hideModal();
        }

        public void ClearStage(string voteStage, string voteResult)
        {
            Clients.All.clearStage(voteStage, voteResult);
        }

        public void ClearModal()
        {
            Clients.All.clearModal();
        }

        public void ShowMotion(string motionHeader, string motionText)
        {
            Clients.All.showMotion(motionHeader, motionText);
        }


        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }

        
        public void GoVote(string voteItemId, string votePhase)
        {
            Clients.All.showCurrentVoting(voteItemId);
            Clients.Group("BoardMembers").enableVote(voteItemId, votePhase);
            
        }

        public void IVoted(string voteItemId)
        {
            Clients.Caller.hideCurrentMotion();
            Clients.Group("Manager").recordVote(voteItemId);
            
        }

        public void ShowVoteResult(string result, string tally)
        {
            Clients.All.showVoteResult(result, tally);
        }

        public void InitializeAgenda(string agendaId)
        {
            Clients.All.initializeAgenda(agendaId);
        }

        public void UpdatePosition(string currentSection, string currentAgendaItem)
        {
            Clients.All.updatePosition(currentSection, currentAgendaItem);
        }


        public void ChangeQuorem(string isQuorem)
        {
            Clients.Group("Presenter").changeQuorem(isQuorem);
        }

        public void SendManagerAddBoardMember(string name)
        {
            var boardMember = new BoardMember();
            string id = boardMember.GetBoardMemberKey(name, 2);

            Clients.Group("Manager").addBoardMember(name, id);
        }

        public void SendManagerRemoveBoardMember(string name)
        {
            var boardMember = new BoardMember();
            string id = boardMember.GetBoardMemberKey(name, 2);
            Clients.Group("Manager").removeBoardMember(name, id);
        }

        public void SendTech(string name, string message)
        {
            Clients.Group("TechSupport").addNewMessageToPage(name, message);
            
        }

        public async Task JoinRoom(string user, string roomName)
        {
            SendTech(user, " is joining " + roomName);
            if (roomName == "BoardMembers") { SendManagerAddBoardMember(user);}
            userDictionary.Add(Context.ConnectionId, user);
            
            await Groups.Add(Context.ConnectionId, roomName);
        }

        public async Task LeaveRoom(string user, string roomName)
        {
            SendTech(user," is leaving " + roomName);
            if (roomName == "BoardMembers") { SendManagerRemoveBoardMember(user); }
            userDictionary.Remove(Context.ConnectionId);
            await Groups.Remove(Context.ConnectionId, roomName);
        }
    }
}