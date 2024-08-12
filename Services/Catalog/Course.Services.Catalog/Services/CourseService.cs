using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using Course.Services.Catalog.Settings;
using Course.Shread.Dtos;
using MongoDB.Driver;
using System.Diagnostics.Eventing.Reader;

namespace Course.Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Coursec> _course;
        private readonly IMongoCollection<Category> _category;

        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _course = database.GetCollection<Coursec>(databaseSettings.CoursesCollectionName);

            _category = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;

            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _course.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _category.Find<Category>(X => X.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }
            else
            {
                courses = new List<Coursec>();
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses),200);
            }
            

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _course.Find<Coursec>(x=>x.Id == id).FirstOrDefaultAsync();

            if(course == null)
            {
                return Response<CourseDto>.Fail("Course not found", 404);
            }
            course.Category = await _category.Find<Category>(x =>x.Id == course.CategoryId).FirstAsync();

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course),200);
        }


        public async Task<Response<List<CourseDto>>> GetAllUserId(string userId)
        {
            var courses = await _course.Find<Coursec>(x => x.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _category.Find<Category>(X => X.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Coursec>();
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }


        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Coursec>(courseCreateDto);

            newCourse.CreatedTime = DateTime.Now;
            await _course.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse),200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Coursec>(courseUpdateDto);

            var result = await _course.FindOneAndReplaceAsync(x => x.Id ==courseUpdateDto.Id, updateCourse);

            if (result == null)
            {
                return Response<NoContent>.Fail("Course not found", 404);

            }

            return Response<NoContent>.Success(204);
        }


        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _course.DeleteOneAsync(x=>x.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);

            }
            else
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }
        }

        }
    }

