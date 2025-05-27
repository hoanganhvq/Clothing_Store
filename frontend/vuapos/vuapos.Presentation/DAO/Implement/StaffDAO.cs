using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DAO.Interface;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.DAO.Implement
{
    public class StaffDAO : IDAO<Staff>
    {
        private static List<Staff> _staffList = new List<Staff>
        {
            new Staff { Staff_Id = "ST001", Username = "alice", Password = "123456", Role = "Manager", Phone = "0123456789" },
            new Staff { Staff_Id = "ST002", Username = "bob", Password = "abcdef", Role = "Coach", Phone = "0987654321" },
            new Staff { Staff_Id = "ST003", Username = "charlie", Password = "pass123", Role = "Trainer", Phone = "0111222333" }
        };

        public List<Staff> GetAll()
        {
            return _staffList;
        }

        public Staff GetById(string id)
        {
            return _staffList.FirstOrDefault(s => s.Staff_Id == id);
        }

        public void Add(Staff entity)
        {
            _staffList.Add(entity);
        }

        public void Update(Staff entity)
        {
            var staff = GetById(entity.Staff_Id);
            if (staff != null)
            {
                staff.Username = entity.Username;
                staff.Password = entity.Password;
                staff.Role = entity.Role;
                staff.Phone = entity.Phone;
            }
        }

        public void Delete(string id)
        {
            var staff = GetById(id);
            if (staff != null)
            {
                _staffList.Remove(staff);
            }
        }
    }
}
