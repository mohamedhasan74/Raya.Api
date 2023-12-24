using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Raya.Api.Dtos;
using Raya.Api.Errors;
using Raya.Core.Entities;
using Raya.Core.Interfaces;
using Raya.Core.Specification;

namespace Raya.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(ApiSuccessResponse), 200)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] ProductSpecParams queryParams)
        {
            var spec = new ProductsByConditionSpec(queryParams);
            var products = await _unitOfWork.Repository<Product>().GetAllAsync(spec);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<Product>>(products);
            return Ok(new ApiSuccessResponse { Success = true, Data = productsDto });
        }

        [ProducesResponseType(typeof(ApiSuccessResponse), 200)]
        [ProducesResponseType(typeof(ApiFailResponse), 404)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product is null) return Ok(new ApiFailResponse { Success = false, Error = new ApiErrorResponse(404, "Product Not Found")});
            var productDto = _mapper.Map<Product, ProductDto>(product);
            return Ok(new ApiSuccessResponse { Success = true, Data = productDto});
        }

        [ProducesResponseType(typeof(ApiSuccessResponse), 200)]
        [ProducesResponseType(typeof(ApiFailResponse), 404)]
        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if(product is null) return Ok(new ApiFailResponse { Success = false, Error = new ApiErrorResponse(404, "Product Not Found")});
            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.CompleteAsyn();
            return Ok(new ApiSuccessResponse { Success = true, Data = "Product Is Deleted"});
        }

        [ProducesResponseType(typeof(ApiSuccessResponse), 200)]
        [ProducesResponseType(typeof(ApiFailResponse), 404)]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductDto productDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productDto.Id);
            if (product is null) return Ok(new ApiFailResponse { Success = false, Error = new ApiErrorResponse(404, "Product Not Found") });
            try {
                _mapper.Map(productDto, product);
                _unitOfWork.Repository<Product>().Update(product);
                await _unitOfWork.CompleteAsyn();
                return Ok(new ApiSuccessResponse { Success = true, Data = "Product Is Updated" });
            }
            catch (DbUpdateException ex)
            {
                return Ok(new ApiFailResponse { Success = false, Error = new ApiErrorResponse(400, "Product Is Already Exists") });
            }
            catch (Exception ex)
            {
                return Ok(new ApiFailResponse { Success = false, Error = new ApiErrorResponse(400, "Error occurred During Update Product") });
            }
        }
        [ProducesResponseType(typeof(ApiSuccessResponse), 200)]
        [ProducesResponseType(typeof(ApiFailResponse), 400)]
        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductAddDto productDto)
        {
            var product = _mapper.Map<ProductAddDto, Product>(productDto);
            try
            {
                await _unitOfWork.Repository<Product>().AddAsync(product);
                await _unitOfWork.CompleteAsyn();
                return Ok(new ApiSuccessResponse { Success = true, Data = "Product Is Added"});
            }
            catch (DbUpdateException ex)
            {
                return Ok(new ApiFailResponse { Success = false, Error = new ApiErrorResponse(400, "Product Is Already Exists") });
            }
            catch (Exception ex)
            {
                return Ok(new ApiFailResponse { Success = false, Error = new ApiErrorResponse(400, "Error occurred During Add Product") });
            }
        }


        
    }
}
