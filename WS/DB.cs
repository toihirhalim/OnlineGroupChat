﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS
{
    public class DB
    {
        DCDataContext DC = new DCDataContext();

        public void Add(string pseudo)
        {
            DC.Participant.InsertOnSubmit(new Participant() { Pseudo = pseudo });
            DC.SubmitChanges();
        }

        public List<Participant> getListParticipants()
        {
            List<Participant> participants;
            participants =  DC.Participant.ToList();
            foreach (Participant p in participants)
            {
                p.Message = null;
                p.Recievers = null;
            }
            
            return participants;
        }

        public bool Exist(string pseudo)
        {
            return DC.Participant.FirstOrDefault(t => t.Pseudo.Equals(pseudo)) != null;
        }

        public void Delete(string pseudo)
        {
            if (pseudo.Equals("")) return;

            DC.Recievers.DeleteAllOnSubmit(DC.Recievers.Where(r => r.Participant.Pseudo.Equals(pseudo) || r.Message.Participant.Pseudo.Equals(pseudo)));
            DC.Message.DeleteAllOnSubmit(DC.Message.Where(m => m.Participant.Pseudo.Equals(pseudo)));
            DC.Participant.DeleteAllOnSubmit(DC.Participant.Where(p => p.Pseudo.Equals(pseudo)));

            DC.SubmitChanges();
        }

        public void addMessage(string sender, string texte, List<string> recievers)
        {
            if (texte.Equals("")) return;

            Participant senderParticipant = DC.Participant.FirstOrDefault(p => p.Pseudo.Equals(sender)); 

            if(senderParticipant != null)
            {
                List<Participant> recieversParticipants = DC.Participant.Where(p => recievers.Contains(p.Pseudo)).ToList();

                if(recieversParticipants.Count() > 0)
                {
                    Message msg = new Message()
                    {
                        Msg = texte,
                        Participant = senderParticipant,
                    };

                    foreach (Participant p in recieversParticipants)
                        msg.Recievers.Add(new Recievers() { Participant = p });
                    
                    DC.Message.InsertOnSubmit(msg);

                    DC.SubmitChanges();
                }
            }
        }

        public Message addMessage(string sender, string texte)
        {
            Participant participant = DC.Participant.FirstOrDefault(p => p.Pseudo.Equals(sender));
            
            if(participant != null && participant.Id != 0)
            {
                Message msg = new Message() { Msg = texte, Participant= participant };

                DC.Message.InsertOnSubmit(msg);
                DC.SubmitChanges();

                msg.Recievers = null;
                msg.Participant = null;
                return msg;
            }

            return null;
        }

        public List<Message> getMessages(string pseudo)
        {
            List<Message> messages = DC.Message.Where(m => m.Participant.Pseudo.Equals(pseudo)).ToList();
            foreach (Message msg in messages)
            {
                msg.Participant = null;
                msg.Recievers = null;
            }
                
            return messages;
        }

        public void Clear()
        {
            DC.Recievers.DeleteAllOnSubmit(DC.Recievers);
            DC.Message.DeleteAllOnSubmit(DC.Message);
            DC.Participant.DeleteAllOnSubmit(DC.Participant);
            DC.SubmitChanges();
        }

        internal void Delete(List<string> pseudos)
        {
            DC.Recievers.DeleteAllOnSubmit(DC.Recievers.Where(r => pseudos.Contains(r.Participant.Pseudo)));
            DC.Message.DeleteAllOnSubmit(DC.Message.Where(m => pseudos.Contains(m.Participant.Pseudo)));
            DC.Participant.DeleteAllOnSubmit(DC.Participant.Where(p => pseudos.Contains(p.Pseudo)));
            DC.SubmitChanges();
        }
    }
}