using System;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Filters
{
    public enum UserRole
    {
        Visitor = 0,
        User = 1,
        Exhibitor = 2,
        Host = 3,
    }
	public class AuthorizeFilter : AuthorizeAttribute
	{
		public UserRole CheckUserRole { get; set; } //要檢查的角色權限

		public AuthorizeFilter(UserRole checkRole)
		{
			CheckUserRole = checkRole;
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			if (httpContext == null)
			{
				throw new ArgumentNullException("httpContext");
			}
			bool CheckResult = false; //false 表示不符合權限

			// 取得用戶角色
			UserRole userRole = UserRole.Visitor; //預設角色
			if (httpContext.Session["UserRole"] != null)
			{
				// 從 Session 取得角色
				userRole = (UserRole)Enum.Parse(typeof(UserRole), httpContext.Session["UserRole"].ToString(), true);
			}

			// 檢查一般用戶權限
			if (CheckUserRole == UserRole.User)
			{
				if (userRole == UserRole.User)
				{
					CheckResult = true;
				}
			}

			// 檢查參展商權限
			if (CheckUserRole == UserRole.Exhibitor)
			{
				if (userRole == UserRole.Exhibitor)
				{
					CheckResult = true;
				}
			}

			// 檢查主辦方權限
			if (CheckUserRole == UserRole.Host)
			{
				if (userRole == UserRole.Host)
				{
					CheckResult = true;
				}
			}
			return CheckResult;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			// 當權限檢查失敗時，跳頁至登入頁
			filterContext.Result = new RedirectResult("~/Home/DenyAuthorize");
		}
	}
}