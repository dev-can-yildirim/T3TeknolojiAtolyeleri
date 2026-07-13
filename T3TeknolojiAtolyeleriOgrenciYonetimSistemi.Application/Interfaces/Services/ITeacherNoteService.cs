using T3TeknolojiAtolyeleriApplication.DTOs.TeacherNoteDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface ITeacherNoteService
    {
        Task<ServiceResponse<List<TeacherNoteListDto>>> GetAllAsync();

        Task<ServiceResponse<TeacherNoteListDto>> GetByIdAsync(int id);

        Task<ServiceResponse<List<TeacherNoteListDto>>> GetByTeacherIdAsync(int teacherId);

        Task<ServiceResponse<List<TeacherNoteListDto>>> GetByAssignmentIdAsync(int teacherCourseAssignmentId);

        Task<ServiceResponse<TeacherNoteListDto>> CreateAsync(CreateTeacherNoteDto dto);

        Task<ServiceResponse<TeacherNoteListDto>> UpdateAsync(UpdateTeacherNoteDto dto);

        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
