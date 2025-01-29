/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./*.{html,js}"],
  theme: {  container: {
    center: true,
    screens: {
      lg: '1140px',
      xl: '1440px',
      
    },
  },

    extend: {
      fontFamily: {
        'montserrat': ['Montserrat'],
        'roboto': ['Roboto'],
        'poppins': ['Poppins'],
        
      },
      spacing:{
        '128':'38rem'
      }
    },
  },
  plugins: [],
}