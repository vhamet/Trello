const gulp = require('gulp');
const sass = require('gulp-sass');

gulp.task('sass', () => gulp.src('Styles/**/*.scss')
  .pipe(sass().on('error', sass.logError))
  .pipe(sass({ outputStyle: 'compressed' }))
  .pipe(gulp.dest('wwwroot/css')));

gulp.task('sass:watch', () => {
  gulp.watch('Styles/**/*.scss', ['sass']);
});
