using AutoMapper;
using Learn.Net.Modal;
using Learn.Net.Repository.Models;

namespace Learn.Net.Helper
{
    public class AutoMapperHandler:Profile
    {
        public AutoMapperHandler()
        {
            //source -> destination
            //convert TblCustomer to CustomerModal
            CreateMap<TblCustomer, CustomerModal>();
            CreateMap<CustomerModal, TblCustomer>();
        }
    }
}
