﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UdemyDotNet.CustomActionFilters;
using UdemyDotNet.Data;
using UdemyDotNet.Models.Domain;
using UdemyDotNet.Models.DTO;
using UdemyDotNet.Repositories;

namespace UdemyDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionController(MyDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from database - domain models
            //var regionsDomain = await dbContext.Regions.ToListAsync(); - before
            //after
            var regionsDomain = await regionRepository.GetAllAsync();

            // map domain models to DTOs using manual
            //var regionsDto = new List<RegionDto>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });
            //}

            // map domain models to dtos using auto mapper
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            //return dtos
            return Ok(regionsDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            
            //var region = dbContext.Regions.Find(Guid.Parse(id));
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null) { 
                return NotFound(); 
            }

            //conver domain model to dtos
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};

            //mapper using automapper
            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto) {
            //map dto to domain model
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
            //};

            //map dto to domain model using auto mapper
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // use Domain model to create region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //map domain model back to dto 
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            //using mapper
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
        }
        [HttpPut]
        [Route("{id:Guid}")]        
        
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //map dto to domain model
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionRequestDto.Code,
            //    Name = updateRegionRequestDto.Name,
            //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            //};

            //using auto mapper
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //check if region exist 
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //convert domain model to dto
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            //using auto mapper
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if(regionDomainModel == null) {
                return NotFound();
            }
            //map domain model back to dto
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            //using auto mapper
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
