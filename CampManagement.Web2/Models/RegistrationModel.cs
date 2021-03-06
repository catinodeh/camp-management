﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using CampManagement.Domain.Entities;
using Microsoft.Owin.Security.DataHandler;

namespace CampManagement.Web.Models
{
    public class RegistrationModel
    {
        public int I { get; set; }
    }

 //   ResultType INT,
 //   GuardianId INT,
	//GuardianName VARCHAR(100),
	//GuardianPhone VARCHAR(10),
	//CamperId INT,
 //   CamperName VARCHAR(200),
	//CamperBirthDate DATE,
 //   CamperPhone VARCHAR(10),
	//CamperLastYear INT

    public class RegistrationSearchModel
    {
        public int ResultType { get; set; }
        public int? GuardianId { get; set; }
        public string GuardianName { get; set; }
        public string GuardianPhone { get; set; }
        public int? CamperId { get; set; }
        public string CamperName { get; set; }
        public DateTime? CamperBirthDate { get; set; }
        public string CamperPhone { get; set; }
        public string CamperGender { get; set; }
        public int? CamperLastYear { get; set; }
    }

    public class AvailableCamps
    {
        public int CampSetupId { get; set; }
        public string Description { get; set; }
        public int FromGrade { get; set; }
        public int ToGrade { get; set; }
        public decimal Price { get; set; }
    }

    public class NewGuardianEmail
    {
        public string first_name { get; set; }
        public string button_text { get; set; }
    }

    public class CurrentSetupModel
    {
        public Guardian Guardian { get; set; }
        public List<RegistrationCamper> Registrations { get; set; }
    }

    public class UpdateRegistrationCamperModel
    {
        public int Grade { get; set; }
        public int CampSetupId { get; set; }
        public decimal Price { get; set; }
    }
}