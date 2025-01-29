/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',

  content: [
    './wwwroot/**/*.html',
    './wwwroot/lib/*.js',
    './Views/**/*.cshtml', 
  ],
  theme: {
    extend: {
      fontFamily: {
        'montserrat': ['Montserrat'],
        'roboto': ['Roboto'],
        'poppins': ['Poppins'],

      },
      height:{
        '1': '2px', 

      },
      opacity: {
        '5': '0.01', 
        '50': '0.5', 
      },
      screens: {
        '2xl': '1600px', 
      },
      spacing: {
        '68': '21rem',

        '114': '23rem',
        '115': '32rem',
        '116': '35rem',
        '128': '42rem',
        '164': '51rem',
        '130': '41rem',
        '134': '48rem'


      }
    },
  },
  plugins: [],
}
