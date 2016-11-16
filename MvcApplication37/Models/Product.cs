using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication37.Models
{
    public class Product
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Display(Name = "Производитель")]
        public string Manufaturer { get; set; }
        [Display(Name = "Модель")]
        public string PhoneModel { get; set; }
        [Display(Name = "Цена")]
        public int Price { get; set; }

        #region Property
        [Display(Name = "Тип")]
        public string Type { get; set; }
        [Display(Name = "Операционная система")]
        public string OS { get; set; }
        [Display(Name = "Версия ОС")]
        public string OSVersion { get; set; }
        [Display(Name = "Диагональ экрана")]
        public string Screen { get; set; }
        [Display(Name = "Разрешение экрана")]
        public string ScreenSize { get; set; }
        [Display(Name = "Оперативная память")]
        public string RAM { get; set; }
        [Display(Name = "Flash память")]
        public string FlasMemory { get; set; }
        [Display(Name = "Две SIM-карты")]
        public bool DoubleSIM { get; set; }
        [Display(Name = "Камера")]
        public int Camera { get; set; }
        [Display(Name = "Процессор")]
        public string Processor { get; set; }
        [Display(Name = "Частота процессора")]
        public string ProcessorClockSpeed { get; set; }
        [Display(Name = "Цвет")]
        public string Color { get; set; }
        [Display(Name = "Материал")]
        public string Material { get; set; }
        [Display(Name = "Ширина")]
        public int Width { get; set; }
        [Display(Name = "Высота")]
        public int Height { get; set; }
        [Display(Name = "Вес")]
        public int Weight { get; set; }
        [Display(Name = "Количество цветов")]
        public long ColoCount { get; set; }
        [Display(Name = "Технология экрана")]
        public string ScreenTechnology { get; set; }
        #endregion
        [Display(Name = "Количество")]
        public int Count { get; set; }
        [Display(Name = "Фотография")]
        public string PhotoUrl { get; set; }
    }
}