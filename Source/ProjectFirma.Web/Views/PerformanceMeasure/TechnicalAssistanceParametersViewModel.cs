﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LtInfo.Common;
using LtInfo.Common.Models;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;
using ProjectFirma.Web.Views.Shared;

namespace ProjectFirma.Web.Views.PerformanceMeasure
{
    public class TechnicalAssistanceParametersViewModel : FormViewModel
    {
        public List<TechnicalAssistanceParameterSimple> TechnicalAssistanceParameters { get; set; }
        
        /// <summary>
        /// Needed by ModelBinder
        /// </summary>
        public TechnicalAssistanceParametersViewModel()
        {

        }

        public TechnicalAssistanceParametersViewModel(List<TechnicalAssistanceParameterSimple> technicalAssitanceParameters)
        {
            TechnicalAssistanceParameters = technicalAssitanceParameters.OrderByDescending(x=>x.Year).ToList();
        }

        public void UpdateModel(List<TechnicalAssistanceParameter> currentTechnicalAssistanceParameters,
            ICollection<TechnicalAssistanceParameter> allTechnicalAssistanceParameters)
        {
            var technicalAssistanceParametersUpdated = new List<TechnicalAssistanceParameter>();

            if (TechnicalAssistanceParameters != null)
            {
                technicalAssistanceParametersUpdated =
                    TechnicalAssistanceParameters.Select(x => x.ToTechnicalAssistanceParameter()).ToList();
            }

            currentTechnicalAssistanceParameters.Merge(technicalAssistanceParametersUpdated,
                allTechnicalAssistanceParameters, (x, y) => x.Year == y.Year, (x, y) =>
                {
                    x.EngineeringHourlyCost = y.EngineeringHourlyCost;
                    x.OtherAssistanceHourlyCost = y.OtherAssistanceHourlyCost;
                });
        }
    }

}

namespace ProjectFirma.Web.Models
{
    public class TechnicalAssistanceParameterSimple
    {
        public int Year { get; set; }

        public Money? EngineeringHourlyCost { get; set; }

        public Money? OtherAssistanceHourlyCost { get; set; }

        /// <summary>
        /// Needed by ModelBinder
        /// </summary>
        public TechnicalAssistanceParameterSimple()
        {

        }

        public TechnicalAssistanceParameterSimple(TechnicalAssistanceParameter technicalAssistanceParameter)
        {
            Year = technicalAssistanceParameter.Year;
            EngineeringHourlyCost = technicalAssistanceParameter.EngineeringHourlyCost;
            OtherAssistanceHourlyCost = technicalAssistanceParameter.OtherAssistanceHourlyCost;
        }

        public TechnicalAssistanceParameterSimple(int year)
        {
            Year = year;
        }

        public TechnicalAssistanceParameter ToTechnicalAssistanceParameter()
        {
            return new TechnicalAssistanceParameter(Year)
            {
                EngineeringHourlyCost = EngineeringHourlyCost,
                OtherAssistanceHourlyCost = OtherAssistanceHourlyCost
            }; 
        }
    }
}