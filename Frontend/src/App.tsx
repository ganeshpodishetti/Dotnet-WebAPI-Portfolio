import 'bootstrap/dist/css/bootstrap.min.css';
import { Route, BrowserRouter as Router, Routes, useLocation } from 'react-router-dom';
import AdminNavbar from './components/admin/AdminNavbar';
import ProtectedRoute from './components/admin/ProtectedRoute';
import Navbar from './components/Navbar';
import About from './pages/About';
import AdminDashboard from './pages/admin/Dashboard';
import AdminEducation from './pages/admin/Education';
import AdminExperience from './pages/admin/Experience';
import AdminLogin from './pages/admin/Login';
import AdminMessages from './pages/admin/Messages';
import AdminProjects from './pages/admin/Projects';
import AdminSkills from './pages/admin/Skills';
import AdminSocialLinks from './pages/admin/SocialLinks';
import AdminUserProfile from './pages/admin/UserProfile';
import Contact from './pages/Contact';
import Education from './pages/Education';
import Experience from './pages/Experience';
import Home from './pages/Home';
import Projects from './pages/Projects';
import Skills from './pages/Skills';

// A component to render the appropriate navbar based on the current path
const NavbarContainer = () => {
  const location = useLocation();
  const isAdminRoute = location.pathname.startsWith('/admin');
  
  // Don't show any navbar on the admin login page
  if (location.pathname === '/admin/login') {
    return null;
  }
  
  return isAdminRoute ? <AdminNavbar /> : <Navbar />;
};

function App() {
  return (
    <Router>
      <AppContent />
    </Router>
  );
}

function AppContent() {
  const location = useLocation();
  const isAdminLogin = location.pathname === '/admin/login';
  
  // Decide which navbar to show based on route
  return (
    <>
      <NavbarContainer />
      
      <div className={`container mt-4 ${isAdminLogin ? 'mt-5' : ''}`}>
        <Routes>
          {/* Public Routes */}
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
          <Route path="/projects" element={<Projects />} />
          <Route path="/experience" element={<Experience />} />
          <Route path="/education" element={<Education />} />
          <Route path="/skills" element={<Skills />} />
          <Route path="/contact" element={<Contact />} />

          {/* Admin Auth Routes */}
          <Route path="/admin/login" element={<AdminLogin />} />
          
          {/* Protected Admin Routes */}
          <Route path="/admin" element={
            <ProtectedRoute>
              <AdminDashboard />
            </ProtectedRoute>
          } />
          <Route path="/admin/projects" element={
            <ProtectedRoute>
              <AdminProjects />
            </ProtectedRoute>
          } />
          <Route path="/admin/education" element={
            <ProtectedRoute>
              <AdminEducation />
            </ProtectedRoute>
          } />
          <Route path="/admin/experience" element={
            <ProtectedRoute>
              <AdminExperience />
            </ProtectedRoute>
          } />
          <Route path="/admin/skills" element={
            <ProtectedRoute>
              <AdminSkills />
            </ProtectedRoute>
          } />
          <Route path="/admin/sociallinks" element={
            <ProtectedRoute>
              <AdminSocialLinks />
            </ProtectedRoute>
          } />
          <Route path="/admin/messages" element={
            <ProtectedRoute>
              <AdminMessages />
            </ProtectedRoute>
          } />
          <Route path="/admin/profile" element={
            <ProtectedRoute>
              <AdminUserProfile />
            </ProtectedRoute>
          } />
        </Routes>
      </div>
    </>
  );
}

export default App;
