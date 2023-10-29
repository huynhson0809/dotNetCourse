using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyDotNet.Models.Domain;
using UdemyDotNet.Models.DTO;
using UdemyDotNet.Repositories;

namespace UdemyDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        public WalksController(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        public IMapper Mapper { get; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //mapping walk dto to domain model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
            walkDomainModel = await walkRepository.CreateAsync(walkDomainModel);

            //map domain model to dto
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            string? sortBy, bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,isAscending ?? true, 
                pageNumber, pageSize);

            //map domain model to dto
            var walksDto = mapper.Map<List<WalkDto>>(walksDomainModel);
            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            //map dto to domain model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            var existingWalk = await walkRepository.UpdateAsync(id, walkDomainModel);
            if(existingWalk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(existingWalk));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingWalkDomainModel = await walkRepository.DeleteAsync(id);
            if(existingWalkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(existingWalkDomainModel));
        }
    }
}
