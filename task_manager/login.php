<?php
include 'includes/header.php';
include 'config/database.php';

if(isset($_SESSION['user_id'])) {
    header("Location: dashboard.php");
    exit();
}

if($_SERVER['REQUEST_METHOD'] == 'POST') {
    $username = trim($_POST['username']);
    $password = $_POST['password'];
    
    $errors = [];
    
    if(empty($username) || empty($password)) {
        $errors[] = "Please fill in all fields";
    }
    
    if(empty($errors)) {
        $stmt = $pdo->prepare("SELECT * FROM users WHERE username = ? OR email = ?");
        $stmt->execute([$username, $username]);
        $user = $stmt->fetch();
        
        if($user && password_verify($password, $user['password'])) {
            $_SESSION['user_id'] = $user['id'];
            $_SESSION['username'] = $user['username'];
            $_SESSION['email'] = $user['email'];
            
            header("Location: dashboard.php");
            exit();
        } else {
            $errors[] = "Invalid username/email or password";
        }
    }
}
?>

<div class="auth-container">
    <div class="auth-form">
        <h2>Login</h2>
        
        <?php if(isset($_SESSION['success'])): ?>
            <div class="success-box">
                <p><?php echo $_SESSION['success']; unset($_SESSION['success']); ?></p>
            </div>
        <?php endif; ?>
        
        <?php if(!empty($errors)): ?>
            <div class="error-box">
                <?php foreach($errors as $error): ?>
                    <p><?php echo $error; ?></p>
                <?php endforeach; ?>
            </div>
        <?php endif; ?>
        
        <form method="POST" action="">
            <div class="form-group">
                <label>Username or Email:</label>
                <input type="text" name="username" value="<?php echo $_POST['username'] ?? ''; ?>" required>
            </div>
            
            <div class="form-group">
                <label>Password:</label>
                <input type="password" name="password" required>
            </div>
            
            <button type="submit" class="btn btn-primary">Login</button>
        </form>
        
        <p>Don't have an account? <a href="register.php">Register here</a></p>
        
        <div class="demo-account">
            <h3>Demo Account:</h3>
            <p>Username: demo</p>
            <p>Password: 123456</p>
        </div>
    </div>
</div>

<?php include 'includes/footer.php'; ?>