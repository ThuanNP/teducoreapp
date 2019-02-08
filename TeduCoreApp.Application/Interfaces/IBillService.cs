using System;
using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IBillService : IDisposable
    {
        void Create(BillViewModel billViewModel);

        void Update(BillViewModel billViewModel);

        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, 
                                                string keyword, int pageIndex, int pageSize);
        void UpdateStatus(int orderId, BillStatus status);
        
        void Save();

        //BillDetail
        BillViewModel GetWithDetails(int orderId);

        List<BillDetailViewModel> GetBillDetails(int orderId);

        BillDetailViewModel CreateDetail(BillDetailViewModel billDetailViewModel);

        void DeleteDetail(int productId, int orderId, int colorId, int sizeId);

        List<ColorViewModel> GetColors();

        List<SizeViewModel> GetSizes();

    }
}
