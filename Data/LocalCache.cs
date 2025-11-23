using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using SticksAndStonesGCApi.Models;


namespace SticksAndStonesGCApi.Data
{
    public class LocalCache : IDisposable
    {
        private readonly string _dbPath;

        public LocalCache(string? dbFolder = null)
        {
            var baseDir = dbFolder ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SticksAndStonesGCApi");
            Directory.CreateDirectory(baseDir);
            _dbPath = Path.Combine(baseDir, "golfCourseCache.db");
        }

        public void SaveCourses(IEnumerable<SourceData> courses)
        {
            using var db = new LiteDatabase(_dbPath);
            var col = db.GetCollection<SourceData>("courses");

            col.InsertBulk(courses);
        }

        public void SaveCourse(SourceData course)
        {
            using var db = new LiteDatabase(_dbPath);

            var col = db.GetCollection<SourceData>("courses");

            col.Upsert(course);
        }

        public List<SourceData> GetCourses()
        {
            using var db = new LiteDatabase(_dbPath);
            var col = db.GetCollection<SourceData>("courses");
            return col.FindAll().ToList();
        }

        public SourceData GetCourse(string courseName)
        {
            using var db = new LiteDatabase(_dbPath);
            var col = db.GetCollection<SourceData>("courses");

            var course = col.FindOne(x => x.Name == courseName);

            return course;
        }

        public void Clear()
        {
            using var db = new LiteDatabase(_dbPath);
            var col = db.GetCollection<SourceData>("courses");
            col.DeleteAll();
        }

        public void Dispose()
        {
            // Nothing to dispose when using LiteDatabase per-call. Keep for DI compatibility.
        }
    }
}
