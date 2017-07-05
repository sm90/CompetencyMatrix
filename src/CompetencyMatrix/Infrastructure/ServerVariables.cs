using System;
using System.Collections.Generic;
using CompetencyMatrix.ViewModels;
using Microsoft.AspNetCore.Http;

namespace CompetencyMatrix.Infrastructure
{
    public class ServerVariables : IServerVariables
    {
        private const string CURRENT_USER_ID = "CurrentUserId";
        private const string CURRENT_ASP_USER_ID = "CurrentAspUserId";
        private const string CURRENT_EDIT_USER_ID = "CurrentEditUserId";
        private const string CURRENT_USER_SKILLS = "CurrentUserSkills";
        private const string CURRENT_USER_POSITION = "CurrentUserPosition";
        private const string SELECTED_POSITION_MATRIX_ID = "SelectedPositionMatrixId";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public ServerVariables(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentAspUserId
        {
            get
            {
                if (string.IsNullOrEmpty(_session.GetString(CURRENT_ASP_USER_ID)))
                {
                    return string.Empty;
                }
                return _session.GetString(CURRENT_ASP_USER_ID);
            }
            set { _session.SetString(CURRENT_ASP_USER_ID, value); }
        }

        public string CurrentUserPosition
        {
            get
            {
                return string.IsNullOrEmpty(_session.GetString(CURRENT_USER_POSITION))
                    ? string.Empty
                    : _session.GetString(CURRENT_USER_POSITION);
            }
            set { _session.SetString(CURRENT_USER_POSITION, value); }
        }

        public EmployeeSkillStorageModel CurrentUserSkills
        {
            get
            {
                if ((_session.GetObjectFromJson<EmployeeSkillStorageModel>(CURRENT_USER_SKILLS)) == null)
                {
                    return null;
                }
                return _session.GetObjectFromJson<EmployeeSkillStorageModel>(CURRENT_USER_SKILLS);
            }
            set
            {
                _session.SetObjectAsJson(CURRENT_USER_SKILLS, value);
            }
        }


        public int CurrentUserId
        {
            get
            {
                int userId;
                if (string.IsNullOrEmpty(_session.GetString(CURRENT_USER_ID)))
                {
                    userId = 0;
                }
                else
                {
                    if (!int.TryParse(_session.GetString(CURRENT_USER_ID), out userId))
                        userId = 0;
                }
                return userId;

            }
            set { _session.SetString(CURRENT_USER_ID, value.ToString()); }
        }


        public int CurrentEditUserId
        {
            get
            {
                int userId;
                if (string.IsNullOrEmpty(_session.GetString(CURRENT_EDIT_USER_ID)))
                    return 0;

                if (!int.TryParse(_session.GetString(CURRENT_EDIT_USER_ID), out userId))
                    userId = 0;

                return userId;

            }
            set { _session.SetString(CURRENT_EDIT_USER_ID, value.ToString()); }
        }
        public int? SelectedPositionMatrixId
        {
            get { return _session.GetInt32(SELECTED_POSITION_MATRIX_ID); }
            set
            {
                if (value.HasValue) _session.SetInt32(SELECTED_POSITION_MATRIX_ID, value.Value);
            }
        }
    }
}