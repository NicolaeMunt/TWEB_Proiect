using System.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TWEB_Proiect.UnityMvcActivator), "Start")]

namespace TWEB_Proiect
{
    public static class UnityMvcActivator
    {
        public static void Start()
        {
            // Пустая реализация - Unity не критичен для работы проекта
        }
    }
}