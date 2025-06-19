using Microsoft.AspNetCore.Mvc;
using TaskManagerMVC.Dto.Auth;
using TaskManagerMVC.Services.Interfaces;

namespace TaskManagerMVC.Controllers
{
    public class RolePermissionController : Controller
    {

        private readonly IAuthService _authService;
        public RolePermissionController(IAuthService authService)
        {
            _authService = authService;
        }

        // Chỉ Admin (RoleId = 1) mới được truy cập
        private async Task<bool> IsAdminAsync()
        {
            var roleId = await _authService.GetUserRoleIdAsync(User);
            return roleId == 1;
        }

        // Hiển thị danh sách vai trò
        [HttpGet]
        public async Task<IActionResult> ViewRole()
        {
/*            if (!await IsAdminAsync())
            {
                return RedirectToAction("AccessDenied", "Login");
            }*/

            var roles = await _authService.GetAllRolesAsync();
            return View(roles);
        }

        // Hiển thị form phân quyền cho vai trò
        [HttpGet]
        public async Task<IActionResult> AssignPermissions(int roleId)
        {
/*            if (!await IsAdminAsync())
            {
                return RedirectToAction("AccessDenied", "Login");
            }*/

            var dto = await _authService.GetRolePermissionAsync(roleId);
            if (dto.RoleId == 0)
            {
                return NotFound();
            }

            return View(dto);
        }

        // Xử lý cập nhật quyền
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignPermissions(RolePermissionDto dto)
        {
/*            if (!await IsAdminAsync())
            {
                return RedirectToAction("AccessDenied", "Login");
            }*/

            if (!ModelState.IsValid)
            {
                dto.AvailablePermissions = (await _authService.GetRolePermissionAsync(dto.RoleId)).AvailablePermissions;
                return View(dto);
            }

            await _authService.UpdateRolePermissionsAsync(dto.RoleId, dto.PermissionIds);
            TempData["Message"] = "Phân quyền thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}