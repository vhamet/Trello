const gulp = require('gulp');
const sass = require('gulp-sass');
const autoprefixer = require('gulp-autoprefixer');
const minify = require('gulp-terser');

gulp.task('sass', () => gulp.src('Styles/**/*.scss')
  .pipe(sass().on('error', sass.logError))
  .pipe(autoprefixer({
    browsers: ['last 99 versions'],
    cascade: false,
  }))
  .pipe(sass({ outputStyle: 'compressed' }))
  .pipe(gulp.dest('wwwroot/css')));

gulp.task('sass:watch', () => {
  gulp.watch('Styles/**/*.scss', gulp.series('sass'));
});

gulp.task('minify', () => gulp.src('js/**/*.js')
  .pipe(minify())
  .pipe(gulp.dest('wwwroot/js')));

gulp.task('minify:watch', () => {
  gulp.watch('js/**/*.js', gulp.series('minify'));
});
