using T3TeknolojiAtolyeleriApplication.DTOs.AttendanceDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface IAttendanceService
    {
        Task<ServiceResponse<List<AttendanceListDto>>> GetByLessonSessionIdAsync(int lessonSessionId);
        Task<ServiceResponse<List<AttendanceListDto>>> SaveAttendanceAsync(SaveAttendanceDto dto);
        Task<ServiceResponse<List<AttendanceWarningDto>>> GetWarningStudentsByTeacherIdAsync(int teacherId);
        Task<ServiceResponse<List<AttendanceWarningDto>>> GetWarningStudentsAsync();
    }
}
