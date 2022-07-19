<script>
  import { Link } from 'yrv'
</script>
<style>
  ul,
  .nav-header {
    display: flex;
    margin: 5px 0;
    padding: 0;
    flex-flow: column nowrap;
    line-height: 0.9rem;
  }
  .nav-header {
    margin: 6px 0 3px;
    padding: 4px 8px 4px 25px;
    text-transform: uppercase;
    font-size: 0.6rem;
    color: rgba(255, 255, 255, 0.6);
  }
  ul > li {
    display: block;
  }
  ul > li > a {
    display: block;
    padding: 4px 8px 4px 20px;
  }
  
  ul > li > a > hr {
    border-color: rgba(255, 255, 255, 0.1);
    margin-right: 45px;
    width: auto;
  }
  ul > li > a:link {
    text-decoration: none;
    color: inherit;
  }
  ul > li > a:hover {
    color: #bccee4;
  }
  ul > li > a > i {
    margin-right: 10px;
  }
</style>

<div>
  <div class="nav-header">Main</div>
  <ul>
    <li><Link href="/" class="link"><i class="fas fa-home" />Home</Link></li>
  </ul>
  <div class="nav-header">Dashboards</div>
  <ul>
    <li><Link href="/product" class="link"><i class="fas fa-project-diagram"></i>Product</Link></li>
    <li>
      <Link href="/" class="link"><i class="fas fa-microchip" />Namespaces</Link>
    </li>
  </ul>
  <!--
  <div class="nav-header">Workloads</div>
  <ul>
    <li><Link href="/deployments" class="link">Deployments</Link></li>
    <li><Link href="/statefulsets" class="link">Stateful Sets</Link></li>
    <li><a href="javascript:void(0)">Daemon Sets</a></li>
    <li><a href="javascript:void(0)">Secrets</a></li>
    <li><a href="javascript:void(0)">Config Maps</a></li>
    <li><Link href="/pods" class="link">Pods</Link></li>

    <li>
      <a href="javascript:void(0)"><hr /></a>
    </li>
    <li><a href="javascript:void(0)">Replica Sets</a></li>
    <li><a href="javascript:void(0)">Horizontal Pod Autoscalers</a></li>
    <li>
      <a href="javascript:void(0)"><hr /></a>
    </li>
    <li><Link href="/ingresses" class="link">Ingresses</Link></li>
    <li><a href="javascript:void(0)">Services</a></li>
  </ul>
  <div class="nav-header">Helm</div>
  <ul>
    <li>
      <a href="javascript:void(0)"><i class="fas fa-life-ring" />Releases</a>
    </li>
  </ul>
  <div class="nav-header">GitHub</div>
  <ul>
    <li>
      <a href="javascript:void(0)"><i class="fab fa-github" />Repositories</a>
    </li>
  </ul>
  -->
</div>