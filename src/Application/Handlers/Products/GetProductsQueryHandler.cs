using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using MediatR;

namespace Application.Handlers.Products
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ApiResponse<List<ProductDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            if (request._Category != "")
                return await GetProductsByCategoryAsync(request._Category, request.GetProductsByCategoryKey);
            else
                return await GetAllProductsAsync(request.GetAllProductsKey);
        }



        private async Task<ApiResponse<List<ProductDto>>> GetProductsByCategoryAsync(string category, string GetProductsByCategoryKey)
        {
            var cachedproductsbycategory = await _cacheService.GetAsync<List<ProductDto>>(GetProductsByCategoryKey);

            if (cachedproductsbycategory != null)
                return new ApiResponse<List<ProductDto>>(cachedproductsbycategory, Status.Success, "Ürünler cacheden getirildi.");
            else
            {
                var products = await _productRepository.GetAllAsync(x => x.Category.ToLower() == category.ToLower() && x.Status == true && x.Unit > 0);
                var productsDto = _mapper.Map<List<ProductDto>>(products);
                await _cacheService.SetAsync(GetProductsByCategoryKey, productsDto);
                return new ApiResponse<List<ProductDto>>(productsDto, Status.Success, "Ürünler databaseden getirildi.");
            }
        }

        private async Task<ApiResponse<List<ProductDto>>> GetAllProductsAsync(string getAllProductsKey)
        {
            var cachedproducts = await _cacheService.GetAsync<List<ProductDto>>(getAllProductsKey);

            if (cachedproducts != null)
                return new ApiResponse<List<ProductDto>>(cachedproducts, Status.Success, "Ürünler cacheden getirildi.");
            else
            {
                var products = await _productRepository.GetAllAsync(x => x.Status == true && x.Unit > 0);
                var productsDto = _mapper.Map<List<ProductDto>>(products);
                await _cacheService.SetAsync(getAllProductsKey, productsDto);
                return new ApiResponse<List<ProductDto>>(productsDto, Status.Success, "Ürünler databaseden getirildi.");
            }
        }
    }
}
