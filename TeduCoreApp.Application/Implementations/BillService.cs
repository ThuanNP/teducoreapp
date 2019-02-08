using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Implementations
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _orderRepository;
        private readonly IBillDetailRepository _orderDetailRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BillService(IBillRepository orderRepository, IBillDetailRepository orderDetailRepository,
            IColorRepository colorRepository, ISizeRepository sizeRepository,
            IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(BillViewModel billViewModel)
        {
            billViewModel.OrderDate = DateTime.Now;
            //Mapping to order and order details domain
            var order = Mapper.Map<BillViewModel, Bill>(billViewModel);
            var orderDetails = Mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billViewModel.BillDetails);
            foreach (BillDetail detail in orderDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
            }
            order.BillDetails = orderDetails;
            _orderRepository.Add(order);
        }

        public void Update(BillViewModel billViewModel)
        {
            //Mapping to order domain
            var order = Mapper.Map<BillViewModel, Bill>(billViewModel);
            //Get order Detail
            var newDetails = order.BillDetails;
            //new details added
            var addedDetails = newDetails.Where(x => x.Id == 0).ToList();
            //get updated details
            var updatedDetails = newDetails.Where(x => x.Id != 0).ToList();
            //Existed details
            var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == billViewModel.Id);
            //Clear db
            order.BillDetails.Clear();
            //Update details
            foreach (BillDetail detail in updatedDetails)
            {
                Product product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Update(detail);
            }
            //Add new details
            foreach (BillDetail detail in addedDetails)
            {
                Product product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Add(detail);
            }
            _orderDetailRepository.RemoveMultiple(existedDetails.Except(updatedDetails).ToList());
            _orderRepository.Update(order);
        }

        public void UpdateStatus(int orderId, BillStatus status)
        {
            Bill order = _orderRepository.FindById(orderId);
            order.BillStatus = status;
            _orderRepository.Update(order);
        }

        public List<SizeViewModel> GetSizes()
        {
            return _sizeRepository.FindAll().ProjectTo<SizeViewModel>().ToList();
        }

        public List<ColorViewModel> GetColors()
        {
            return _colorRepository.FindAll().ProjectTo<ColorViewModel>().ToList();
        }

        public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate,
            string keyword, int pageIndex, int pageSize)
        {
            var query = _orderRepository.FindAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.OrderDate >= start);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.OrderDate <= end);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.CustomerName.Contains(keyword) || x.CustomerMobile.Contains(keyword));
            }
            var totalRow = query.Count();
            query = query.OrderByDescending(x => x.OrderDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
             var data = query.ProjectTo<BillViewModel>().ToList();

            return new PagedResult<BillViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public BillViewModel GetWithDetails(int orderId)
        {
            Bill bill = _orderRepository.FindSingle(x => x.Id == orderId);
            BillViewModel billViewModel = Mapper.Map<Bill, BillViewModel>(bill);
            List<BillDetailViewModel> billDetailViewModels = _orderDetailRepository.FindAll(x => x.BillId == orderId).ProjectTo<BillDetailViewModel>().ToList();
            billViewModel.BillDetails = billDetailViewModels;
            return billViewModel;
        }

        public List<BillDetailViewModel> GetBillDetails(int orderId)
        {
            return _orderDetailRepository
                .FindAll(x => x.BillId == orderId, c => c.Bill, c => c.Color, c => c.Size, c => c.Product)
                .ProjectTo<BillDetailViewModel>().ToList();
        }

        public BillDetailViewModel CreateDetail(BillDetailViewModel billDetailViewModel)
        {
            var billDetail = Mapper.Map<BillDetailViewModel, BillDetail>(billDetailViewModel);
            _orderDetailRepository.Add(billDetail);
            return billDetailViewModel;
        }

        public void DeleteDetail(int productId, int orderId, int colorId, int sizeId)
        {
            var detail = _orderDetailRepository.FindSingle(x => 
                                    x.ProductId == productId
                                    && x.BillId == orderId
                                    && x.ColorId == colorId 
                                    && x.SizeId == sizeId);
            _orderDetailRepository.Remove(detail);
        }

        public void Save() => _unitOfWork.Commit();

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
