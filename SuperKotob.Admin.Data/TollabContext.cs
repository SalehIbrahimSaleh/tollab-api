namespace SuperKotob.Admin.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Models;
     using Tollab.Admin.Data.Models;
    using Tollab.Admin.Data.Models.Views;

    public partial class TollabContext : DbContext
    {
        public TollabContext()
            : base("name=TollabContext")
        {
        }
         public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Role> Roles   { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<OfflinePackage> OfflinePackages { get; set; }
        public virtual DbSet<ContactUs>  ContactUs { get; set; }
        public virtual DbSet<ContentType> ContentTypes  { get; set; }
        public virtual DbSet<Content> Contents  { get; set; }
        public virtual DbSet<Course> Courses  { get; set; }
        public virtual DbSet<CourseDepartment> CourseDepartments  { get; set; }
        public virtual DbSet<Department> Departments  { get; set; }
        public virtual DbSet<Favourite> Favourites  { get; set; }
        public virtual DbSet<Group> Groups  { get; set; }
        public virtual DbSet<NotificationType>  NotificationTypes  { get; set; }
        public virtual DbSet<Offer> Offers  { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods  { get; set; }
        public virtual DbSet<PromoCode>PromoCodes   { get; set; }
        public virtual DbSet<Reply> Replies   { get; set; }
        public virtual DbSet<SearchWord> SearchWords   { get; set; }
        public virtual DbSet<Section> Sections   { get; set; }
        public virtual DbSet<Student> Students   { get; set; }
        public virtual DbSet<StudentContent> StudentContents   { get; set; }
        public virtual DbSet<StudentCourse> StudentCourses   { get; set; }
        public virtual DbSet<StudentPackage> StudentPackages { get; set; }
        public virtual DbSet<StudentLive> StudentLives { get; set; }
        public virtual DbSet<StudentDepartment> StudentDepartments   { get; set; }
        public virtual DbSet<StudentNotification> StudentNotifications   { get; set; }
        public virtual DbSet<StudentPromoCode> StudentPromoCodes   { get; set; }
        public virtual DbSet<StudentTransaction> StudentTransactions   { get; set; }
        public virtual DbSet<SubCategory> SubCategories   { get; set; }
        public virtual DbSet<SystemTransaction> SystemTransactions   { get; set; }
        public virtual DbSet<Teacher> Teachers   { get; set; }
        public virtual DbSet<TeacherAssistant>  TeacherAssistants   { get; set; }
        public virtual DbSet<TeacherAccount> TeacherAccounts   { get; set; }
        public virtual DbSet<TeacherNotification> TeacherNotifications   { get; set; }
        public virtual DbSet<TeacherSubject> TeacherSubjects   { get; set; }
        public virtual DbSet<TeacherTransaction> TeacherTransactions   { get; set; }
        public virtual DbSet<TermAndCondition>  TermAndConditions   { get; set; }
        public virtual DbSet<Track>   Tracks   { get; set; }
        public virtual DbSet<TrackPromotion> TrackPromotions { get; set; }
        public virtual DbSet<TrackPromotionCourse> TrackPromotionCourses { get; set; }
        public virtual DbSet<VideoQuestion> VideoQuestions { get; set; }
        public virtual DbSet<Reference>  References { get; set; }
        public virtual DbSet<SystemAdmin>   SystemAdmins { get; set; }
        public virtual DbSet<StudentPushToken>    StudentPushTokens { get; set; }
        public virtual DbSet<TeacherPushToken>     TeacherPushTokens { get; set; }
        public virtual DbSet<TrackSubscription> TrackSubscriptions { get; set; }
        public virtual DbSet<Setting>  Settings { get; set; }
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }
        public virtual DbSet<SystemNotification>   SystemNotifications { get; set; }

        public virtual DbSet<HomeStatistics>   HomeStatistics { get; set; }
        public virtual DbSet<CountryStatistics> CountryStatistics { get; set; }
        public virtual DbSet<TopTeachers> TopTeachers { get; set; }
        public virtual DbSet<TopCourses> TopCourses { get; set; }
        public virtual DbSet<StudentSubject> StudentSubjects { get; set; }
        public virtual DbSet<TeachersSubject>  TeachersSubjects { get; set; }
        public virtual DbSet<PaymentMethodType>   PaymentMethodTypes { get; set; }
        public virtual DbSet<AboutUs> AboutUs { get; set; }
        public virtual DbSet<StudentExam> StudentExams  { get; set; }
        public virtual DbSet<StudentAnswer> StudentAnswers  { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions  { get; set; }
        public virtual DbSet<ExamAnswer> ExamAnswers  { get; set; }
        public virtual DbSet<Exam> Exams  { get; set; }
        public virtual DbSet<CowPayLog> CowPayLogs  { get; set; }
        public virtual DbSet<ExamType> ExamTypes  { get; set; }
        public virtual DbSet<ExamQuestionType> ExamQestionTypes  { get; set; }
        public virtual DbSet<UserDeviceLog> UserDeviceLogs { get; set; }
        public virtual DbSet<StudentCourseView> StudentCourseViews { get; set; }
        public virtual DbSet<StudentExamView> StudentExamViews { get; set; }
        public virtual DbSet<DisableReason>  DisableReasons { get; set; }
        public virtual DbSet<OfferCountry> OfferCountries{ get; set; }
        public virtual DbSet<Live> Lives{ get; set; }
        public virtual DbSet<LiveAttachment>  LiveAttachments{ get; set; }

        //

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //new MigrateDatabaseToLatestVersion<ODS_DbContext, Configuration>()
            Database.SetInitializer<TollabContext>(null);        

        }

        public System.Data.Entity.DbSet<Tollab.Admin.Data.Models.Subject> Subjects { get; set; }

        public System.Data.Entity.DbSet<Tollab.Admin.Data.Models.CourseStatus> CourseStatuses { get; set; }
    }
}
