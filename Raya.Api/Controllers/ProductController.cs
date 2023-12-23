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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            return Ok(productsDto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product is null) return NotFound(new ApiErrorResponse(404, "Product Was Not Found"));
            var productDto = _mapper.Map<Product, ProductDto>(product);
            return Ok(productDto);
        }
        [HttpGet("FilterByName/{name}")]
        public async Task<ActionResult<ProductDto>> GetAllByName(string name)
        {
            var spec = new BaseSpecification<Product>(P => P.Name.ToLower().Contains(name.ToLower()));
            var products = await _unitOfWork.Repository<Product>().GetAllByConditionAsync(spec);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            return Ok(productsDto);
        }
        [HttpGet("FilterByPrice/{price}")]
        public async Task<ActionResult<ProductDto>> GetAllByName(int price)
        {
            var spec = new BaseSpecification<Product>(P => P.Price == price);
            var products = await _unitOfWork.Repository<Product>().GetAllByConditionAsync(spec);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            return Ok(productsDto);
        }
        [HttpDelete]
        public async Task<ActionResult> RemoveProduct(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if(product is null) return NotFound(new ApiErrorResponse(404, "Product Was Not Found"));
            _unitOfWork.Repository<Product>().Remove(product);
            await _unitOfWork.CompleteAsyn();
            return Ok(new { message = "Product Is Deleted" });
        }
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductDto productDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productDto.Id);
            if (product is null) return NotFound(new ApiErrorResponse(404, "Product Was Not Found"));
            try {
                _mapper.Map(productDto, product);
                _unitOfWork.Repository<Product>().Update(product);
                await _unitOfWork.CompleteAsyn();
                return Ok(new { message = "Product Is Updated" });
            } catch(DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException &&
                (sqlException.Number == 2601 || sqlException.Number == 2627))
                return BadRequest(new ApiErrorResponse(400, "Product Is Already Exists"));
                return BadRequest(new ApiErrorResponse(400));
            } catch(Exception ex)
            {
              return BadRequest(new ApiErrorResponse(400));
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductAddDto productDto)
        {
            var product = _mapper.Map<ProductAddDto, Product>(productDto);
            try
            {
                await _unitOfWork.Repository<Product>().AddAsync(product);
                await _unitOfWork.CompleteAsyn();
                return Ok(new { message = "Product Is Added" });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException &&
                (sqlException.Number == 2601 || sqlException.Number == 2627))
                    return BadRequest(new ApiErrorResponse(400, "Product Is Already Exists"));
                return BadRequest(new ApiErrorResponse(400));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(400));
            }
        }

    }
}
