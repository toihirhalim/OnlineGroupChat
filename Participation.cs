﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discussion
{
    public partial class Participation : Form
    {
        srv.ws_chatSoapClient srv = new srv.ws_chatSoapClient();
        public string Pseudo { get; set; }
        List<srv.Participant> Participants { get; set; }

        public Participation()
        {
            InitializeComponent();
        }

        public Participation(string pseudo, srv.Participant [] participants)
        {
            InitializeComponent();
            Pseudo = pseudo;
            Participants = participants.ToList();
        }

        private void Participation_Load(object sender, EventArgs e)
        {
            Text = "Mon pseudo est " + Pseudo;
            lstParticipants.DataSource = Participants.Where(p => !p.Pseudo.Equals(Pseudo)).ToList();
            lstParticipants.DisplayMember = "pseudo";
        }
        private void Participation_Close(object sender, EventArgs e)
        {
            srv.Quiter(Pseudo);
        }

    }
}